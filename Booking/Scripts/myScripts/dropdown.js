var dropDown = document.getElementById("drop");
var btn = document.getElementById("button");

btn.addEventListener('click', function () {
    dropDown.classList.toggle('active');
});