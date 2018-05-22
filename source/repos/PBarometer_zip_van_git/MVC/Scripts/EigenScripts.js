function grafiekWijzigen(aanTePassenGrafiek) {
  
  
    var titel = $("#inputTitel").val();
    var typeViewbag;


    var grafiekIdViewbag = aanTePassenGrafiek.GrafiekId;

    if (titel === null) {
        titelViewbag = aanTePassenGrafiek.Titel;
    } else {
        var titelViewbag = titel;
    }


    var type = aanTePassenGrafiek.Type;
    if (type === "bar") {
        if (document.getElementById('chkAssenWisselen').checked) {
            typeViewbag = "horizontalBar";
        } else {
            typeViewbag = aanTePassenGrafiek.Type;
        }
    } else {
        typeViewbag = aanTePassenGrafiek.Type;
    }


    var toonLegendeViewbag = aanTePassenGrafiek.ToonLegende;
    var toonXAsViewbag = aanTePassenGrafiek.ToonXAs;
    var xOorsprongNulViewbag = aanTePassenGrafiek.XOorsprongNul;
    var yOorsprongNulViewbag = aanTePassenGrafiek.YOorsprongNul;

    var toonYAsViewbag = aanTePassenGrafiek.ToonYAs;
    var xTitelViewbag = aanTePassenGrafiek.XTitel;
    var yTitelViewbag = aanTePassenGrafiek.YTitel;

    var xLabelsViewbag = aanTePassenGrafiek.XLabels;

    var dataViewbag = aanTePassenGrafiek.Datawaarden;
    var legendelijstViewbag = aanTePassenGrafiek.LegendeLijst;

    var backgroundcolorViewbag = aanTePassenGrafiek.Achtergrondkleur;
    var bordercolorViewbag = aanTePassenGrafiek.Randkleur;


    var FillDatasetViewbag = aanTePassenGrafiek.FillDataset;
    var LijnlegendeweergaveViewbag = aanTePassenGrafiek.Lijnlegendeweergave;
    var XAsMaxrotatieViewbag = aanTePassenGrafiek.XAsMaxrotatie;
    var XAsMinrotatieViewbag = aanTePassenGrafiek.XAsMinrotatie;




    GrafiekOpbouwen(grafiekIdViewbag, titelViewbag, typeViewbag, toonLegendeViewbag, xOorsprongNulViewbag, yOorsprongNulViewbag, toonXAsViewbag, toonYAsViewbag,
        FillDatasetViewbag, LijnlegendeweergaveViewbag, XAsMaxrotatieViewbag, XAsMinrotatieViewbag, xTitelViewbag, yTitelViewbag,
        xLabelsViewbag, legendelijstViewbag[0], legendelijstViewbag[1], legendelijstViewbag[2], legendelijstViewbag[3], legendelijstViewbag[4],
        dataViewbag[0], dataViewbag[1], dataViewbag[2], dataViewbag[3], dataViewbag[4],
        backgroundcolorViewbag[0], backgroundcolorViewbag[1], backgroundcolorViewbag[2], backgroundcolorViewbag[3], backgroundcolorViewbag[4],
        bordercolorViewbag[0], bordercolorViewbag[1], bordercolorViewbag[2], bordercolorViewbag[3], bordercolorViewbag[4]
    );

}


function grafiekVerwijderen(id) {
    var lengteId = id.substring(id.indexOf("-") + 1, id.length - 1).length;
    var subid;

    if (lengteId === 0) {
        subid = id.substring(id.length - 1);
    } else if (lengteId > 0) {
        subid = id.substring(id.indexOf("-") + 1, id.length - 1);
    }

    var grafiekVerwijderen = confirm("Ben je zeker dat je de grafiek wilt verwijderen?");
    if (grafiekVerwijderen === true) {
        $("canvas#" + subid).remove()

        $("#verwijderen-" + subid).remove();
        $("#bewerken-" + subid).remove();

    }
}




function GrafiekOpbouwen(id, titel, grafiektype, toonLegende = true, xAsNul = true, yAsNul = true, toonXAs = true, toonYAs = true, datasetFill, lijnweergave,
    xAsMaxRotatie = 90, xAsMinRotatie = 0, xTitel, yTitel,
    labels, label1 = null, label2 = null, label3 = null, label4 = null, label5 = null,
    data1, data2 = null, data3 = null, data4 = null, data5 = null,
    backgroundcolor1 = null, backgroundcolor2 = null, backgroundcolor3 = null, backgroundcolor4 = null, backgroundcolor5 = null,
    bordercolor1 = null, bordercolor2 = null, bordercolor3 = null, bordercolor4 = null, bordercolor5 = null) {


    var grafiekdata;
    var grafiekopties;
    var aantalDatasets;

    if (data1 !== null && data2 === null && data3 === null && data4 === null && data5 === null) {
        aantalDatasets = 1;
    } else if (data1 !== null && data2 !== null && data3 === null && data4 === null && data5 === null) {
        aantalDatasets = 2;
    } else if (data1 !== null && data2 !== null && data3 !== null && data4 === null && data5 === null) {
        aantalDatasets = 3;
    } else if (data1 !== null && data2 !== null && data3 !== null && data4 !== null && data5 === null) {
        aantalDatasets = 4;
    } else if (data1 !== null && data2 !== null && data3 !== null && data4 !== null && data5 !== null) {
        aantalDatasets = 5;
    }

    switch (aantalDatasets) {
        case 1:
            grafiekdata = {
                labels: labels,
                datasets: [
                    {
                        label: label1,
                        data: data1,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor1,
                        borderColor: bordercolor1,
                        fill: datasetFill

                    }
                ]
            }
            break;

        case 2:
            grafiekdata = {
                labels: labels,
                datasets: [
                    {
                        label: label1,
                        data: data1,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor1,
                        borderColor: bordercolor1,
                        fill: datasetFill

                    },

                    {
                        label: label2,
                        data: data2,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor2,
                        borderColor: bordercolor2,
                        fill: datasetFill

                    },
                ]
            }
            break;

        case 3:
            grafiekdata = {
                labels: labels,
                datasets: [
                    {
                        label: label1,
                        data: data1,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor1,
                        borderColor: bordercolor1,
                        fill: datasetFill

                    },

                    {
                        label: label2,
                        data: data2,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor2,
                        borderColor: bordercolor2,
                        fill: datasetFill

                    },
                    {
                        label: label3,
                        data: data3,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor3,
                        borderColor: bordercolor3,
                        fill: datasetFill

                    }
                ]
            }
            break;

        case 4:
            grafiekdata = {
                labels: labels,
                datasets: [
                    {
                        label: label1,
                        data: data1,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor1,
                        borderColor: bordercolor1,
                        fill: datasetFill

                    },

                    {
                        label: label2,
                        data: data2,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor2,
                        borderColor: bordercolor2,
                        fill: datasetFill

                    },
                    {
                        label: label3,
                        data: data3,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor3,
                        borderColor: bordercolor3,
                        fill: datasetFill

                    },

                    {
                        label: label4,
                        data: data4,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor4,
                        borderColor: bordercolor4,
                        fill: datasetFill

                    }
                ]
            }
            break;

        case 5:
            grafiekdata = {
                labels: labels,
                datasets: [
                    {
                        label: label1,
                        data: data1,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor1,
                        borderColor: bordercolor1,
                        fill: datasetFill

                    },

                    {
                        label: label2,
                        data: data2,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor2,
                        borderColor: bordercolor2,
                        fill: datasetFill

                    },
                    {
                        label: label3,
                        data: data3,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor3,
                        borderColor: bordercolor3,
                        fill: datasetFill

                    },

                    {
                        label: label4,
                        data: data4,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor4,
                        borderColor: bordercolor4,
                        fill: datasetFill

                    },
                    {
                        label: label5,
                        data: data5,
                        borderWidth: 1,
                        backgroundColor: backgroundcolor5,
                        borderColor: bordercolor5,
                        fill: datasetFill

                    }
                ]
            }
            break;
    }

    grafiekopties = {


        title: {
            display: true,
            text: titel,
            fontSize: 18
        },

        legend: {
            display: toonLegende,
            labels: {
                useLineStyle: lijnweergave
            }
        },

        scales: {
            xAxes: [{
                display: toonXAs,
                ticks: {
                    beginAtZero: xAsNul,
                    maxRotation: xAsMaxRotatie,
                    minRotation: xAsMinRotatie
                },
                scaleLabel: {
                    display: true,
                    labelString: xTitel
                }
            }],

            yAxes: [{
                display: toonYAs,
                ticks: {
                    beginAtZero: yAsNul
                },
                scaleLabel: {
                    display: true,
                    labelString: yTitel
                }
            }]
        }
    };




    //var grafiekNieuw = new Chart(ctx, {
    //    options: grafiekopties,
    //    type: grafiektype,
    //    data: grafiekdata
    //});
 //if (window.bar !== undefined) {
    //    window.bar.destroy();
    //window.bar
    //}

    var ctx = $("canvas#" + id);
   

    new Chart(ctx, {
        options: grafiekopties,
        type: grafiektype,
        data: grafiekdata
    });
}


function statistiekVerwijderen(id) {
    var lengteId = id.substring(id.indexOf("-") + 1, id.length - 1).length;
    var subid;

    if (lengteId === 0) {
        subid = id.substring(id.length - 1);
    } else if (lengteId > 0) {
        subid = id.substring(id.indexOf("-") + 1, id.length - 1);
    }


    var statistiekVerwijderen = confirm("Ben je zeker dat je de statistiek wilt verwijderen?");
    if (statistiekVerwijderen === true) {
        $("div#" + subid).remove()

        $("#verwijderen-" + subid).remove();


    }
}