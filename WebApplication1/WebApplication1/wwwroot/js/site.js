// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function eventCheckBox() {
    let checkbox = document.getElementById("selectAll");
    let checkboxs = document.getElementsByTagName("input");
    var k = checkbox.checked;
    for (let i = 1; i < checkboxs.length; i++) {
        checkboxs[i].checked = k;
    }
    checkbox.checked = k;
}