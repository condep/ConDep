namespace ConDep.Dsl
// Learn more about F# at http://fsharp.net


module Module1 =
    let x = ConDep.Dsl.Executor.Execute (fun setup -> 
        setup.Infrastructure.IIS.Define (fun iisDef -> 
            iisDef.WebSite("MyWebSite", 5, @"C:\tmep\mywebsite", (fun webSiteOpt ->
                webSiteOpt.ApplicationPool("MyAppPool")
                webSiteOpt.HttpBinding(8080)
                webSiteOpt.HttpsBinding(444, "myCert")
                webSiteOpt.WebApp("MyWebApp1")
                webSiteOpt.WebApp("MyWebApp2")
            ))          
        )
    )
    