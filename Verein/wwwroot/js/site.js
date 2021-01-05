// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(document).ready(function(){
    $("#modalSearch").click(function() {
      var value = $("#modalInput").val().toLowerCase();
      $("#modalTable tr").filter(function() {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
      });
    });
  });

$(document).ready(function(){
    $("#modalSearch2").click(function() {
      var value = $("#modalInput2").val().toLowerCase();
      $("#modalTable2 tr").filter(function() {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
      });
    });
  });


$(document).ready(function(){
  if(document.getElementById("Mitglied_MitgliedsNummer") != null) {
    $("#Mitglied_MitgliedsNummer").attr("data-val", "false");
    $("#Mitglied_MitgliedsNummer").removeAttr("data-val-required");
  }
});


/*
 * Table filtering and sorting
 */
$(document).ready(function(){
  $("#mitgliederSearch").keyup(function() {
    let input = document.getElementById("mitgliederSearch");
    let filter = input.value.toLowerCase();

    $("#mitgliederTable tbody tr").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(filter) > -1)
    });
  });
});

$(document).ready(function(){
  $("#zahlungsinformationenSearch").keyup(function() {
    let input = document.getElementById("zahlungsinformationenSearch");
    let filter = input.value.toLowerCase();

    $("#zahlungsinformationenTable tbody tr").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(filter) > -1)
    });
  });
});

$(document).ready(function () {
    $("#hundeSearch").keyup(function () {
        let input = document.getElementById("hundeSearch");
        let filter = input.value.toLowerCase();

        $("#hundeTable tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(filter) > -1)
        });
    });
});

$(document).ready(function(){
  const getCellValue = (tr, idx) => tr.children[idx].innerText || tr.children[idx].textContent;

  const comparer = (idx, asc) => (a, b) => ((v1, v2) =>
      v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2) ? v1 - v2 : v1.toString().localeCompare(v2)
      )(getCellValue(asc ? a : b, idx), getCellValue(asc ? b : a, idx));

  // do the work...
  document.querySelectorAll('th').forEach(th => {
    th.style.cursor = "pointer";
    th.addEventListener('click', (() => {
      const table = th.closest('table');
      const tbody = table.querySelector('tbody');
      Array.from(tbody.querySelectorAll('tr'))
        .sort(comparer(Array.from(th.parentNode.children).indexOf(th), this.asc = !this.asc))
        .forEach(tr => tbody.appendChild(tr) );
      })
    );
  });
});





  /*
  $(document).ready(
    function()
    {
        const myOptions = [
            {
                label: "New York",
                value: "NY",
            },
            {
                label: "Washington",
                value: "WA",
            },
            {
                label: "California",
                value: "CA",
            },
            {
                label: "New Jersey",
                value: "NJ",
            },
            {
                label: "North Carolina",
                value: "NC",
            },
        ];

        var instance = new SelectPure(".example", {
            options: myOptions,
            multiple: true, // default: false
            autocomplete: true, // default: false
            placeholder: false,
            icon: "fa fa-times", // uses Font Awesome
            onChange: value => { console.log(value); },
        });
    }
)
*/