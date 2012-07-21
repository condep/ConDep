{
    WebSites : 
    {
        "agent.frende.no" : {
            "localhost": 
            [
                { BindingType: "https", Port : "4430", Ip : "10.70.148.10", HostHeader : ""},
                { BindingType: "http", Port : "8081",  Ip : "10.70.148.11", HostHeader : ""}
            ]
        },
        "sikker.frende.no" : {
            "localhost": 
            [
                { BindingType: "https", Port : "443", Ip : "10.70.148.12", HostHeader : ""},
                { BindingType: "http", Port : "80",  Ip : "10.70.148.13", HostHeader : ""}
            ]
        },
        "tjenester.frende.no" : {
            "localhost": 
            [
                { BindingType: "https", Port : "443", Ip : "10.70.148.14", HostHeader : ""},
                { BindingType: "http", Port : "80",  Ip : "10.70.148.15", HostHeader : ""}
            ]
        }
    }
}