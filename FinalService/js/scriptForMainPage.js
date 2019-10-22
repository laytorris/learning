$(document).ready(function () {  
   PageActions.LoadGridData();
 });   


function DoPageActions(){

  this.ReloadGrid= function(dataUrl){
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
  this.ReloadGridDefault= function(){
   $.ajax({  
    url: 'ContactService.svc/GetAllContacts',  
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
 document.getElementById("showAllButton").classList.add('btn-hidden');
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
           { name: 'ID', index: 'ID', width: 100, hidden:true},  
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
       
      var dataUrl = 'ContactService.svc/GetAllContacts';
      this.ReloadGrid(dataUrl); 
   }
    this.OpenCreatingPage = function(){
        window.open("index.html","_blank");
    }

    this.OpenEditPage = function(){
       if (window.SelectedRowID != undefined){
      var url = "contactUpdatePage.html?ContactID=" + window.SelectedRowID;
      window.open(url,"_blank");
       }
       else{
          alert ("Строка не выбрана");
       }
     
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
var PageActions = new DoPageActions();