﻿using Booking.Models;
using System.Data.Entity;
using Org.BouncyCastle.Asn1.Ocsp;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace Booking.Controllers
{
    public class PaypalController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session["payment"] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session["payment"] as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.
            try
            {
                int idPrenotazione = (int)Session["IdPrenotazione"];
                EmailController.Mail(idPrenotazione, db);
                TempData["Successo"] = "";
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante l'invio dell'email: " + ex.Message;
            }
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            int idPrenotazione = (int)Session["IdPrenotazione"];
            Prenotazione prenotazione = db.Prenotazione.Find(idPrenotazione);

            //create itemlist and add item objects to it
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()
            {
                //name = "Item Name comes here",
                //currency = "EUR",
                //price = "1",
                //quantity = "1",
                //sku = "sku",
                name = "Prenotazione",
                currency = "EUR",
                price = prenotazione.Totale.ToString("0.00", CultureInfo.InvariantCulture),
                quantity = "1",
                sku = prenotazione.IdPrenotazione.ToString(),
            });
            var payer = new Payer()
            {
                payment_method = "paypal",
            };
            // Configure Redirect Urls here with RedirectUrls object
            var rediUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details
            //var details = new Details()
            //{</span>
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "1"
            //};
            //Final amount with details
            var amount = new Amount()
            {
                currency = "EUR",
                total = prenotazione.Totale.ToString("0.00", CultureInfo.InvariantCulture),
                //total = "1",
                //Total must be equal to sum of tax, shipping and subtotal
                //details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice number
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = rediUrls
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult CreditCard()
        {
            return View();
        }

        public ActionResult FailureView()
        {
            return View();
        }
    }
}