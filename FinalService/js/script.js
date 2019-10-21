$( document ).ready(function() {
    dataLoader.LoadOrganizationList();
});

function LoadFieldsValues(){

    this.LoadOrganizationList = function(){
        $.ajax({
            type: "POST",
            url:  "ContactService.svc/GetOrganizationList",
            processData: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success : this.HandleResponse,
            error: function(response) {
                alert(response.responseJSON.Message);
            }
        })
    }
    this.HandleResponse =  function(response){
        var responseobject = JSON.parse(response.d);
        var jobSelect = document.getElementById('InputJob');
        responseobject.forEach(function(element) {
            jobSelect.append(new Option(element.Name, element.ID, false, false));
           });
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


 function ServiceCaller(){
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

function OpenPage(){
    this.OpenCreatingPage = function(){
        window.open("index.html","_self");
    }
}
var PageLinker = new OpenPage();
     