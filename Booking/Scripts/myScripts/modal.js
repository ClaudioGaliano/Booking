const modals = document.querySelectorAll(".mymodal");
const showBtns = document.querySelectorAll(".show-modal");
const closeBtns = document.querySelectorAll(".close-btn");
const overlays = document.querySelectorAll(".overlay");

showBtns.forEach((btn, index) => {
    btn.addEventListener("click", () => {
        modals[index].classList.add("active-modal");
    });
});

closeBtns.forEach((btn, index) => {
    btn.addEventListener("click", () => {
        modals[index].classList.remove("active-modal");
    });
});

overlays.forEach((overlay, index) => {
    overlay.addEventListener("click", () => {
        modals[index].classList.remove("active-modal");
    });
});