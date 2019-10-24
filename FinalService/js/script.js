$(document).ready(function () {  
    PageActions.LoadGridStructure();
    document.getElementById("showAllButton").classList.add("d-none");
    document.getElementById("ContactID").classList.add("d-none");
    var dataUrl = 'ContactService.svc/GetAllContacts';
    PageActions.FillGrid(dataUrl);
     
    PageActions.SetListeners();
  });   

function LoadFieldsValues(){

    this.LoadOrganizationList = function(){
        $.ajax({
            type: "POST",
            url:  "ContactService.svc/GetOrganizationList",
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : function(response){
                var responseobject = JSON.parse(response.d);
                var jobSelect = document.getElementById('InputJob');
                jobSelect.innerHTML="";
                responseobject.forEach(function(element) {
                    jobSelect.append(new Option(element.Name, element.ID, false, false));
                   });
            },
            error: function(response) {
                alert(response.responseJSON.Message);
            }
        })
    }
    this.LoadContactFields = function(ContactID){
        $.ajax({
            type: "POST",
            url:  "ContactService.svc/GetContact",
            data: JSON.stringify({ id: ContactID }),
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : this.SetFieldsValues,
            error: function(response) {
                alert(response.responseJSON.Message);
            }
        })
    }
   
    this.SetFieldsValues =  function(response){
        var responseobject = JSON.parse(response.d);
        document.getElementById("ContactID").value = responseobject.ID;
        document.getElementById("InputName").value = responseobject.Name;
        document.getElementById("InputSurname").value= responseobject.Surname;
        document.getElementById("InputMiddleName").value= responseobject.MiddleName;
        document.getElementById("InputGender").value= responseobject.Gender;
        document.getElementById("InputBirthdate").value= responseobject.BirthDate; 
        document.getElementById("InputPhone").value= responseobject.Phone;
        document.getElementById("InputTaxnumber").value= responseobject.TaxNumber;
        document.getElementById("InputPosition").value= responseobject.Position;
        document.getElementById("InputJob").value= responseobject.Job.ID;
    }
    this.ClearFields =  function() {
        document.getElementById("ContactID").value ="";
        document.getElementById("InputName").value ="";
        document.getElementById("InputSurname").value ="";
        document.getElementById("InputMiddleName").value="";
        document.getElementById("InputGender").value="";
        document.getElementById("InputBirthdate").value=""; 
        document.getElementById("InputPhone").value="";
        document.getElementById("InputTaxnumber").value="";
        document.getElementById("InputPosition").value="";
        document.getElementById("InputJob").value="";
    }
}

var dataLoader  = new LoadFieldsValues();

function ValidateAction(){

    this.ValidateName = function(name){
        let length = name.length;
      
            field = document.getElementById("InputName");
            if (length>10){
                colorChanger.ToIncorrectColor(field);
                console.log("Name is incorrect");
            }
            else{
                colorChanger.ToCorrectColor(field);
            }
            field.value= name[0].toUpperCase() + name.slice(1);
        
    }
    
    this.ValidateSurname = function (surname){
        let length = surname.length;
            field = document.getElementById("InputSurname");
            if (length>15){
                colorChanger.ToIncorrectColor(field);
                console.log("Surname is incorrect");
            }
            else{
                colorChanger.ToCorrectColor(field);
            }
            field.value= surname[0].toUpperCase() + surname.slice(1);
        
    }
    this.ValidateMiddleName = function (middlename){
        let length = middlename.length;
            field = document.getElementById("InputMiddleName");
            if (length>15){
                colorChanger.ToIncorrectColor(field);
                console.log("Middlename is incorrect");
            }
            else{
                colorChanger.ToCorrectColor(field);
            }
            field.value= middlename[0].toUpperCase() + middlename.slice(1);
        
    }
    this.ValidateBirthdate = function(){
           /* let birthdate = document.getElementById("InputBirthdate").value;
            birthdate = new Date(birthdate);
        
            if (!( !!month.value && !!day.value && !!year.value)){
                if(day.value==""){
                    colorChanger.ToIncorrectColor(day);
                }
                if(month.value==""){
                    colorChanger.ToIncorrectColor(month);
                }
                if(year.value==""){
                    colorChanger.ToIncorrectColor(year);
                }
            }
            else{
                if(birthdate =='Invalid Date'){
                    colorChanger.ToIncorrectColor(day);
                    colorChanger.ToIncorrectColor(month);
                    colorChanger.ToIncorrectColor(year);
                    console.log("Date is invalid");
                }
                else{
                    if (birthdate < new Date("01-01-1900") ){
                    console.log("Birthday is incorrect");
                    }
                
                }
            }   
    }
*/
    
}
}
var Validator = new ValidateAction();

function ColorChanger(){
    
    this.ToCorrectColor = function(field){
        field.classList.remove('border-danger');
    }
    this.ToIncorrectColor = function(field){
        field.classList.add('border-danger');
    }
}
var colorChanger = new ColorChanger();


function ContactsActions(){
    this.SentNewContact = function(){
        $.ajax({
            type: "POST",
            url:  "ContactService.svc/InsertContact",
            data: JSON.stringify(this.GetFormData()),
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : function(response) {
                alert('Contact added');
                PageActions.ChangeFormToEdit(response.d);
            },
            error: function(response) {
                alert(response.responseJSON.Message);
            }
        })
    }

    this.UpdateContact = function(){
        $.ajax({
            type: "POST",
            url:  "ContactService.svc/UpdateContact",
            data: JSON.stringify(this.GetFormData()),
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : function() {
                alert('Contact edited');
            },
            error: function(response) {
                alert(response.responseJSON.Message);
            }
        })
    }

    this.DeleteContact= function(){
        if (window.SelectedRowID != undefined){
           if(confirm("Вы уверены, что хотите удалить выбранную строку?")){ 
           $.ajax({
              type: "POST",
              url:  "ContactService.svc/DeleteContact",
              data: JSON.stringify({id:window.SelectedRowID}),
              processData: false,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success : function() {
                  alert('Contact deleted');
                  PageActions.SetDefaultValues(); 
              },
              error: function(response) {
                  alert(response.responseJSON.Message);
              }
              })
            }
           }
            else{
               alert ("Строка не выбрана");
            }
     
        }

    this.GetFormData = function(){
        var bodyArray = {
            Name: document.getElementById("InputName").value,
            Surname: document.getElementById("InputSurname").value,
            MiddleName: document.getElementById("InputMiddleName").value,
            Gender: document.getElementById("InputGender").value,
            BirthDate: document.getElementById("InputBirthdate").value, 
            Phone: document.getElementById("InputPhone").value, 
            TaxNumber: document.getElementById("InputTaxnumber").value, 
            Position: document.getElementById("InputPosition").value,
            JobID: document.getElementById("InputJob").value,
            ID: document.getElementById("ContactID").value
        }
        return bodyArray
    }
    
}

var contactsActions = new ContactsActions();

function MainPageActions(){

    this.SetListeners = function(){
        document.addEventListener('click', function(event) {
        var specifiedElement = document.getElementById("list");
        var isClickInside = specifiedElement.contains(event.target);
        if (!isClickInside) {
            window.SelectedRowID = undefined;
         }
        });
    }
      
    this.FillGrid= function(dataUrl){
       $.ajax({  
        url: dataUrl,  
        type: "POST",  
        contentType: "application/json; charset=utf-8",  
        dataType: "json",  
        success: function (data, st) {  
           if (st == "success" && JSON.parse(data.d.indexOf("_Error_") != 0)) {  
              var grid = $("#list")[0];  
              grid.addJSONData(JSON.parse(data.d));  
           }  
        },  
        error: function () {  
           alert("Error while getting contacts list");  
        }  
     });

    }

    this.SetDefaultValues = function(){
        var dataUrl = 'ContactService.svc/GetAllContacts';
        this.FillGrid(dataUrl); 
        document.getElementById("showAllButton").classList.add("d-none");
        document.getElementById("SearchString").value="";
    }

    this.LoadGridStructure =  function(){
    $("#list").jqGrid({  
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },  
        prmNames: {  
            rows: "numRows",  
            page: "pageNumber"  
        },  
        colNames: ['№', 'Имя', 'Фамилия', 'Дата рождения', 'ИНН', 'Должность', 'Место работы'],
        colModel: [  
            { name: 'ID', index: 'ID', width: 100, hidden : true},  
            { label: 'Name', name: 'Name', index: 'Name', width: 300 },  
            { name: 'Surname', index: 'Surname', width: 300 },  
            { name: 'BirthDate', index: 'BirthDate', width: 100}, 
            { name: 'TaxNumber', index: 'TaxNumber', width: 100 },  
            { name: 'Position', index: 'Position', width: 100 },  
            { name: 'Job.Name', index: 'Organization', width: 100}  
        ], 
        sortname: 'Surname',
        viewrecords: true, 
        sortorder: "desc", 
        multiselect: false,  
        rowNum: 20, 
        rownumbers: true, 
        loadonce: false,  
        autowidth: false,  
        shrinkToFit: false,  
        height: '100%', 
        width:'100%', 
        rowList: [10, 20, 30, 50, 100],  
        sortable: true, 
        onSelectRow: function(){ 
            var rowId = $('#list').jqGrid('getGridParam', 'selrow');
            var rowData = jQuery("#list").getRowData(rowId);
            var colData = rowData['ID']; 
            window.SelectedRowID = colData;
        },
    }).navGrid("#divPaging", { search: true, edit: false, add: false, del: false }, {}, {}, {}, { multipleSearch: false });
    }

    this.OpenAddBlock = function(){
        dataLoader.ClearFields();
       dataLoader.LoadOrganizationList();
       document.getElementById("myModalLabel").innerHTML = "New contact";
       document.getElementById('saveButton').onclick = function(){contactsActions.SentNewContact()};

    }
  
    this.OpenEditBlock = function(){
        if (window.SelectedRowID != undefined){
            document.getElementById("myModalLabel").innerHTML = "Edit contact";
            dataLoader.LoadOrganizationList();
            dataLoader.LoadContactFields(window.SelectedRowID); 
            $("#myModal").modal('show');
            document.getElementById('saveButton').onclick = function(){contactsActions.UpdateContact()};
         }
         else{
            alert ("Строка не выбрана");
         }
    }
    this.ChangeFormToEdit = function(id){
        document.getElementById("myModalLabel").innerHTML = "Edit contact";
        document.getElementById("ContactID").value = id;
        document.getElementById('saveButton').onclick = function(){contactsActions.UpdateContact()};

     }
    
     this.SearchContact =  function(){
        var str =  $("#SearchString").val();
        if ((str.length>=3)&&(str.length<=30)){
           var dataUrl = 'ContactService.svc/GetContactsByParts';
           $.ajax({
              type: "POST",
              url:  dataUrl,
              data: JSON.stringify({searchString:str}),
              processData: false,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success : function(data, st) {
                if (st == "success" && JSON.parse(data.d.indexOf("_Error_") != 0)) {  
                    var grid = $("#list")[0];  
                    grid.addJSONData(JSON.parse(data.d));  
                }
             },
              error: function(response) {
                  alert(response.responseJSON.Message);
              }
            })
              document.getElementById("showAllButton").classList.remove('d-none');
        } 
        else{
            alert("Incorrect search string");
        }
     }
    }
var PageActions = new MainPageActions();