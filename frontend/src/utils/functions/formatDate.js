const formatDate = (data) => {
    // let aux = data + "T03:24:00";

    let dia = new Date(data).getDate();
    let mes = new Date(data).getMonth();
    let ano = new Date(data).getFullYear();

    let hora = new Date(data).getHours();
    let s = "Bom dia";

    let fullMes = "";
    switch (mes) {
        case 0:
            fullMes = "Janeiro";
            break;
        case 1:
            fullMes = "Fevereiro";
            break;
        case 2:
            fullMes = "Março";
            break;
        case 3:
            fullMes = "Abril";
            break;
        case 4:
            fullMes = "Maio";
            break;
        case 5:
            fullMes = "Junho";
            break;
        case 6:
            fullMes = "Julho";
            break;
        case 7:
            fullMes = "Agosto";
            break;
        case 8:
            fullMes = "Setembro";
            break;
        case 9:
            fullMes = "Outubro";
            break;
        case 10:
            fullMes = "Novembro";
            break;
        case 11:
            fullMes = "Dezembro";
            break;

        default:
            break;
    }

    switch (mes) {
        case 0:
            mes = "Jan";
            break;
        case 1:
            mes = "Fev";
            break;
        case 2:
            mes = "Mar";
            break;
        case 3:
            mes = "Abr";
            break;
        case 4:
            mes = "Mai";
            break;
        case 5:
            mes = "Jun";
            break;
        case 6:
            mes = "Jul";
            break;
        case 7:
            mes = "Ago";
            break;
        case 8:
            mes = "Set";
            break;
        case 9:
            mes = "Out";
            break;
        case 10:
            mes = "Nov";
            break;
        case 11:
            mes = "Dez";
            break;

        default:
            break;
    }

    if (hora > 12 && hora <= 0) s = "Bom dia";
    if (hora >= 12 && hora < 18) s = "Boa tarde";
    if (hora >= 18 && hora < 24) s = "Boa noite";

    let weekDay = new Date(data).getDay();
    switch (weekDay) {
        case 0:
            weekDay = "domingo";
            break;
        case 1:
            weekDay = "segunda-feira";
            break;
        case 2:
            weekDay = "terça-feira";
            break;
        case 3:
            weekDay = "quarta-feira";
            break;
        case 4:
            weekDay = "quinta-feira";
            break;
        case 5:
            weekDay = "sexta-feira";
            break;
        case 6:
            weekDay = "sábado";
            break;

        default:
            break;
    }

    return { data: dia + " de " + mes + " " + ano, greeting: s, fullMes: fullMes, weekDay: weekDay };
};

export default formatDate;
