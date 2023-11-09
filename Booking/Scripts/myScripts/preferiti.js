$('.preferito').click(function () {
    var idStruttura = $(this).data('idstruttura');
    var button = $(this);  // Salva il riferimento al pulsante

    $.ajax({
        url: '/Preferiti/AggiungiPreferito',
        type: 'POST',
        data: { id: idStruttura },
        success: function (result) {
            if (result.success) {
                if (result.action === 'added') {
                    alert('Preferito aggiunto con successo!');
                    button.text('Rimuovi dai preferiti');
                } else if (result.action === 'removed') {
                    alert('Preferito rimosso con successo!');
                    button.text('Aggiungi ai preferiti');
                }
            } else {
                if (result.action === 'login') {
                    alert('Effettua il login per aggiungere ai preferiti.');
                } else {
                    alert('Errore nell\'aggiunta del preferito.');
                }
            }
        }
    });
});