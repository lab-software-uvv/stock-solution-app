const backend = () => {
    let link;

    //ip local
    link = "https://stocksolutionapp.azurewebsites.net/api"
    //link = "https://localhost:7156/api"
    // link = "http://192.168.15.11:3001"
    //production
    // link = "https://prod.com";

    return link;
};

export default backend();
