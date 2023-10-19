﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).on('ready' function () {
    $modal = $('.modal-frame');
    $overlay = $('.modal-overlay');

    /* Need this to clear out the keyframe classes so they dont clash with each other between ener/leave. Cheers. */
    $modal.bind('webkitAnimationEnd oanimationend msAnimationEnd animationend', function (e) {
        if ($modal.hasClass('state-leave')) {
            $modal.removeClass('state-leave');
        }
    });

    $('.close').on('click', function () {
        $overlay.removeClass('state-show');
        $modal.removeClass('state-appear').addClass('state-leave');
    });

    $('.open').on('click', function () {
        $overlay.addClass('state-show');
        $modal.removeClass('state-leave').addClass('state-appear');
    });
  

});

// add uporabnik

(function () {
    'use strict'
    const forms = document.querySelectorAll('.requires-validation')
    Array.from(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()

// date picker

const getDatePickerTitle = elem => {
    // From the label or the aria-label
    const label = elem.nextElementSibling;
    let titleText = '';
    if (label && label.tagName === 'LABEL') {
        titleText = label.textContent;
    } else {
        titleText = elem.getAttribute('aria-label') || '';
    }
    return titleText;
}

const elems = document.querySelectorAll('.datepicker_input');
for (const elem of elems) {
    const datepicker = new Datepicker(elem, {
        'format': 'dd/mm/yyyy', // UK format
        title: getDatePickerTitle(elem)
    });
}

//dropdown to label/div

// Get references to the dropdown, button, and display area
const dropdown = document.getElementById('dropdown');
const showDataButton = document.getElementById('showDataButton');
const displayData = document.getElementById('displayData');

// Add a click event listener to the button
showDataButton.addEventListener('click', function () {
    // Get the selected option's value and text
    const selectedValue = dropdown.value;
    const selectedText = dropdown.options[dropdown.selectedIndex].text;

    // Display the selected data in the display area
    displayData.textContent = `Selected Value: ${selectedValue}, Selected Text: ${selectedText}`;
});




///////////////////////// upload file

const fileInput = document.getElementById('file-input');
const uploadButton = document.getElementById('upload-button');
const fileDisplay = document.getElementById('file-display');
const uploadedImage = document.getElementById('uploaded-image');

uploadButton.addEventListener('click', () => {
    const file = fileInput.files[0];
    if (file) {
        const reader = new FileReader();

        reader.onload = function (e) {
            uploadedImage.src = e.target.result;
            fileDisplay.style.display = 'block';
        };

        reader.readAsDataURL(file);
    }
};
