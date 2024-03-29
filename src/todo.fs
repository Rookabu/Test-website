namespace Components

open Feliz
open Feliz.Bulma
open Fable.SimpleJson

//Hier ist die Todo definiert

type TodoElement = {
    Eintrag: string
    Checkbox: bool 
}

//state: liste aus record typen
//typen in json objekt umschreiben 
module SubComponents =
    let tablehead = 
        Html.thead [
            Html.tr [
                Html.th "Eintrag"
                Html.th "Erledigt"
                Html.th "Löschen"
            ]
        ]
        
type Todo =
    [<ReactComponent>]
    static member Main() = 
    
         //ANSATZ: neue funktion schreiben die wie settable etwas an den hinzufügen button schickt
        let (input, setinput) = React.useState("") //setinput soll den input(string) neu setzen. 
        // let (check, setcheck) = React.useState (false)
        let beispiel = [{Eintrag = "Beispiel: Einkaufen gehen"; Checkbox = false }]
        let setLocalStorage (key:string) (info: string) =
            Browser.WebStorage.localStorage.setItem (key, info)

        let getLocalStorage (key: string) =
            Browser.WebStorage.localStorage.getItem (key)

        let backfromstring input= Json.parseAs<TodoElement list> (getLocalStorage input)


        let isLocalStorageClear () =
            match Browser.WebStorage.localStorage.getItem("Eintrag") with
            | null -> true // Local storage is clear if the item doesn't exist
            | _ -> false
 
        
        let initialwert =
            if isLocalStorageClear () then beispiel
            else backfromstring "Eintrag"            


        let (table, settable) = React.useState(initialwert) //table ist ein state der geupdatet wird ruch settable

        let megaSet (nextTable: TodoElement list) =
            let JSONString = Json.stringify nextTable //tabelle wird zu einem string convertiert
            Browser.Dom.console.log (JSONString) 
            setLocalStorage "Eintrag" JSONString //local storage wird gesettet
            settable nextTable //aktualisiert table

        // let removeElementFromTable (elementToRemove: int) =
        //     let mutable newTable = []
        //     for element in table do
        //         if element.Number <> elementToRemove then
        //             newTable <- newTable @ [element] //wenn das jeweilige element nicht dem elementtoremove entspricht, 
        //     newTable                                //wird dieses zum table hinzugefügt. So entsteht eine neue liste ohne das jeweilige element

        Html.div [
            prop.className "childstyle"
            prop.children [    
                Html.h1 [                
                    color.isWhite
                    prop.text "Erstelle dir eine Todo-Liste! Du kannst die Einträge abhaken und wieder entfernen."
                    prop.style [
                        style.marginLeft (length.rem 2)
                        style.marginBottom (length.rem 5)
                        style.fontSize 20
                    ]
                ] 
                                    
                Bulma.control.div [
                    Bulma.input.text [
                        prop.placeholder "Was möchtest du tun?" //Der string der hier eingegeben wird soll gespeichert werden und den button "Eintrag hinzufügen" geshickt werden welcher mit propon click den table neu settet
                        prop.onChange (fun (x:string)->  setinput x)                            
                        prop.style [
                            style.fontSize 20
                            style.width 700
                        ]            
                    ]
                ]
                Html.div [
                    Bulma.button.button [
                        color.isInfo;
                        prop.text "Eintrag hinzufügen"
                        prop.onClick (fun _ -> ( 
                            //setLocalStorage "Eintrag" JSONString
                                megaSet ({Eintrag = input; Checkbox = false} ::table) 
                            //Jstr ing wird in web console geprinted
                            //tabelle soll als string local gespeichert (set) werden und bei wieder aufruf der seite geholt werden (get). Da die liste kein string ist muss es als json zu einem string umgewandelt werden                   
                            ))
                        prop.style [
                            style.fontSize 20
                        ]
                    ]               
                ]            
                Html.div [                     
                    Bulma.table [   
                        SubComponents.tablehead                                      
                        Html.tbody [ 
                            for i in [0 .. (table.Length - 1)]  do //für jedes Element in table wird folgendes gemacht:
                            let element = List.item i table //holt von dem aktuellem index i das Element
                            Html.tr [
                                    Html.td [
                                        prop.text element.Eintrag                                    
                                        if element.Checkbox = true then 
                                            prop.style[
                                                style.textDecorationLine.lineThrough
                                                style.color.gray
                                            ]
                                        ]
                                    Html.td [
                                        Bulma.control.div [
                                            Bulma.input.checkbox [ //Wenn die checkbox angeklickt wurde (true) soll dies über MegaSet gespeichert werden, aber immernoch veränderbar sein
                                                prop.onCheckedChange (fun (isChecked:bool) ->  //on checked change reagiert nicht auf klick, sondern auf en zustand der checkbox
                                                    log isChecked
                                                    // Wenn gechecked wird und x = true ist dann soll das element auf true gesetzt werden und gechecked sein. 
                                                    //Beim wiederaufrufen sollen diese immernoch gechecked sein (prop,onclick). Mit List.map über alles mappen
                                                    table
                                                    |> List.mapi (fun indx item -> //List.mapi: Wenn indx dem aktellen element mit dem index i entspricht then soll dieses item                                                                           //
                                                        if indx = i then           //gelogt werden und das item wird geupdatet zu einer checkbox ischecked. IsChecked ist abhängig ob gecheked (true) oder nicht (false)
                                                            log item
                                                            {item with Checkbox=isChecked}
                                                        else 
                                                            item //wenn der aktuelle index nicht i entspricht dann soll das item (TodoElement) so bleiben wie es ist 
                                                    ) 
                                                    |> megaSet  
                                                )                                                   
                                                prop.isChecked (
                                                    element.Checkbox                                               
                                                )
                                            ]
                                        ]
                                    ]
                                    Html.td [
                                        Bulma.delete [
                                            delete.isMedium
                                            prop.onClick (fun _ ->                                            
                                            table |> List.removeAt i |> megaSet
                                            )
                                        ]
                                    ]
                                ]
                            
                        ]
                    ]
                ]
                Html.div[
                    Html.div [
                        Bulma.button.button [
                        color.isLink;
                        prop.text "Speicher zurücksetzen"
                        prop.onClick (fun _ -> (
                            Browser.WebStorage.localStorage.removeItem "Eintrag" 
                            ))
                        prop.style[
                            style.fontSize 20
                        ]
                        ]               
                    ]
                ]          
            ]
        ]
            
        
                
            
            
            
                
            
        
  

        
        


    