$( document ).ready(function() {
    dataLoader.LoadOrganizationList();
});

function LoadFieldsValues(){

    this.LoadOrganizationList = function(){
        $.ajax({
            type: "POST",
            url:  "http://localhost/FinalService/ContactService.svc/GetOrganizationList",
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : this.HandleResponse,
            error: function() {
                alert('ajax error');
            }
        })
    }
    this.HandleResponse =  function(response){
        var responseobject = JSON.parse(response.d);
        var jobSelect = document.getElementById('InputJob');
        // for (let  item in responseobject) {
          
        // }
       responseobject.forEach(function(element) {
            console.log(element);
           });
        // $.each(responseobject, function (key, value) {
        //     jobSelect.append($('<option></option>').attr('value', ID).text(Name));
        //   })
        //jobSelect.options[jobSelect.options.length] = new Option('Text 1', 'Value1');
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

$(document).ready(function () {  
    $("#list").jqGrid({  
       ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },  
       prmNames: {  
          rows: "numRows",  
          page: "pageNumber"  
       },  
       colNames: ['Имя', 'Фамилия', 'Дата рождения', 'ИНН', 'Должность', 'Место работы'],
       colModel: [  
          { name: 'Name', index: 'Name', width: 400 },  
          { name: 'Surname', index: 'Surname', width: 400 },  
          { name: 'BirthDate', index: 'BirthDate', width: 200}, 
          { name: 'TaxNumber', index: 'TaxNumber', width: 200 },  
          { name: 'Position', index: 'Position', width: 300 },  
          { name: 'Organization', index: 'Organization', width: 300}  
       ], 
    //    datatype: function (postdata) {  
    //       var dataUrl = 'http://localhost/FinalService/Service1.svc/GetData'  
    //       $.ajax({  
    //          url: dataUrl,  
    //          type: "POST",  
    //          contentType: "application/json; charset=utf-8",  
    //          dataType: "json",  
    //          data: JSON.stringify(postdata),  
    //          success: function (data, st) {  
    //             if (st == "success" && JSON.parse(data.d.indexOf("_Error_") != 0)) {  
    //                var grid = $("#divMyGrid")[0];  
    //                grid.addJSONData(JSON.parse(data.d));  
    //             }  
    //          },  
    //          error: function () {  
    //             alert("Error while processing your request");  
    //          }  
    //       });  
    //    },  
       sortname: 'id', //the column according to which data is to be sorted(optional)  
       viewrecords: true, //if true, displays the total number of records, etc. as: "View X to Y out of Z” (optional)  
       sortorder: "asc", //sort order(optional)  
       caption: "Контакты", //Sets the caption for the grid. If this parameter is not set the Caption layer will be not visible  
       multiselect: true,  
       rowNum: 20,  
       loadonce: false,  
       autowidth: true,  
       shrinkToFit: true,  
       height: '100%',  
       rowList: [10, 20, 30, 50, 100],  
       sortable: true  
    }).navGrid("#divPaging", { search: true, edit: false, add: false, del: false }, {}, {}, {}, { multipleSearch: false });  
 });   

 function ServiceCaller(){
    this.SentNewContact = function(){
        $.ajax({
            type: "POST",
            url:  "http://localhost/FinalService/ContactService.svc/InsertContact",
            data: JSON.stringify(this.GetFormData()),
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        })
    }
    this.UpdateContact = function(){
        $.ajax({
            type: "POST",
            url:  "http://localhost/FinalService/ContactService.svc/UpdateContact",
            data: JSON.stringify(this.GetFormData()),
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        })
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
            JobID: document.getElementById("InputJob").value
        }
        return bodyArray
    }
    
}
var ServiceCaller= new ServiceCaller();
     