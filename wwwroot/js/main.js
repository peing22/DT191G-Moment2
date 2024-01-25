"use strict";

// Funktion som körs när HTML-dokumentet har laddats in
document.addEventListener("DOMContentLoaded", function () {

    // Hämtar den aktuella sökvägen till sidan
    let currentPath = window.location.pathname;

    // Hämtar alla länkar i navigeringsmenyn
    let navLinks = document.querySelectorAll("nav ul li a");

    // Loopar igenom länkarna
    navLinks.forEach(function (link) {

        // Hämtar attributet href från varje länk
        let linkPath = link.getAttribute("href");

        // Om href matchar den aktuella sökvägen eller om href är "/lagg-till-traningspass" och den aktuella sökvägen är "/kansla-vid-aktivitet" eller "/summering"
        if (linkPath === currentPath || linkPath === "/lagg-till-traningspass" && (currentPath === "/kansla-vid-aktivitet" || currentPath === "/summering")) {

            // Lägger till klassen active
            link.classList.add("active");
        }
    });
});