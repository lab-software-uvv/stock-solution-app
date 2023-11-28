const getMensagemErroApi = (err) => {
    debugger;
    if (!err.response) {
        return "Ocorreu um erro... Tente novamente!";
    }

    const status = err.response.status;
    switch (status) {
        case 401:
            return "Você não tem permissão para acessar esse recurso!";
        case 404:
            return "Não foi possível encontrar o recurso solicitado!";
        case 500:
            return "Erro interno no servidor!";
        case 400:
            break;
        default:
            return "Ocorreu um erro... Tente novamente!";
    }

    const errors = err.response.data.errors;

    let messages = [];
    for(let key in errors) {
        messages.push(...errors[key]);
    }

    return messages.join('\n\n');
}


export default getMensagemErroApi;
