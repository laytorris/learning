var dialog;
$(document).ready(function () {  
    PageActions.LoadGridData();
    document.getElementById("showAllButton").classList.add("btn-hidden");
    dialog =  document.getElementById("showAllButton").dialog({
        uiLibrary: 'bootstrap4',
        iconsLibrary: 'fontawesome',
        autoOpen: false,
        resizable: false,
        modal: true
    });
    var dataUrl = 'ContactService.svc/GetAllContacts';
    PageActions.FillGrid(dataUrl); 
   
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
        contactID = responseobject.ID;
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
    this.ValidateBirthdate = function(){

            let day =  document.getElementById("birthdateDay");
            let month =  document.getElementById("birthdateMonth");
            let year =   document.getElementById("birthdateYear");
        
            
            colorChanger.ToCorrectColor(day);
            colorChanger.ToCorrectColor(month);
            colorChanger.ToCorrectColor(year);
            
        
            let birthdate = month.value + "-"+day.value  + "-"+year.value;
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
    
}

var Validator = new ValidateAction();

function ColorChanger(){
    
    this.ToCorrectColor = function(field){
        field.classList.add('correct-field-data');
        field.classList.remove('incorrect-field-data');
    }
    this.ToIncorrectColor = function(field){
        field.classList.add('incorrect-field-data');
        field.classList.remove('correct-field-data');
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
            success : function() {
                alert('Contact added');
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
                  var dataUrl = 'ContactService.svc/GetAllContacts';
                  PageActions.ReloadGrid(dataUrl); 
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
            ID: window.ContactID
        }
        return bodyArray
    }
    
}

var contactsActions = new ContactsActions();

function MainPageActions(){

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
        $("#showAllButton").classList.add("btn-hidden");
        $("#SearchString").value="";
    }

    this.LoadGridData =  function(){
    $("#list").jqGrid({  
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },  
        prmNames: {  
            rows: "numRows",  
            page: "pageNumber"  
        },  
        colNames: ['№', 'Имя', 'Фамилия', 'Дата рождения', 'ИНН', 'Должность', 'Место работы'],
        colModel: [  
            { name: 'ID', index: 'ID', width: 100, hidden : true},  
            { name: 'Name', index: 'Name', width: 400 },  
            { name: 'Surname', index: 'Surname', width: 400 },  
            { name: 'BirthDate', index: 'BirthDate', width: 200}, 
            { name: 'TaxNumber', index: 'TaxNumber', width: 200 },  
            { name: 'Position', index: 'Position', width: 300 },  
            { name: 'Job.Name', index: 'Organization', width: 300}  
        ], 
        sortname: 'Surname',
        viewrecords: true, 
        sortorder: "desc", 
        caption: "Контакты",
        multiselect: false,  
        rowNum: 20, 
        rownumbers: true, 
        loadonce: false,  
        autowidth: true,  
        shrinkToFit: true,  
        height: '100%',  
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

    this.OpenAddingBlock = function(){
        dialog.open();
    }
  
    this.OpenEditBlock = function(){
        if (window.SelectedRowID != undefined){
         }
         else{
            alert ("Строка не выбрана");
         }
    }
    
     this.SearchContact =  function(){
        var str =  $("#SearchString").val();
        if (str!=""){
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
              document.getElementById("showAllButton").classList.remove('btn-hidden');
        } 
     }
  }
var PageActions = new MainPageActions();
     