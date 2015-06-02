


function ValidateTextBoxForDataTypeEmailFormat() {
    var today = new Date();



    if (document.getElementById('ctl00_MainContent_EmailID_TX_Y').value.trim() != "") {
        var email = document.getElementById('ctl00_MainContent_EmailID_TX_Y');
        if (!allValidEmailChars(email.value)) { // check to make sure all characters are valid

            return false;

        }

        var tomatch = /^([a-zA-Z0-9]+([\._-][a-zA-Z0-9]+)*)@(([a-zA-Z0-9]+((\.|[-]{1,2})[a-zA-Z0-9]+)*)\.[a-zA-Z]{2,6})$/

        if (email.value != "") {

            if (tomatch.test(email.value)) {

                return true;

            }

            else {


                alert('Please Enter valid EmailId (example@domain.com)');
                document.getElementById('ctl00_MainContent_EmailID_TX_Y').value = '';
                return false;

            }
        }
    }

}

function allValidEmailChars(email) {

    var parsed = true;

    var validchars = "abcdefghijklmnopqrstuvwxyz0123456789@.-_";

    for (var i = 0; i < email.length; i++) {

        var letter = email.charAt(i).toLowerCase();

        if (validchars.indexOf(letter) != -1)

            continue;

        parsed = false;

        break;

    }

    return parsed;

}

function ValidateTestGroup(txtgroupName, chkTest, lblerror) {
    var message = "";
    if (document.getElementById(txtgroupName).value.trim() == "") {
        message = "Please enter Test Group Name.";


    }
    var boolval = false;
    var CHK = document.getElementById(chkTest);
    var checkbox = CHK.getElementsByTagName("input");
    var counter = 0;
    for (var i = 0; i < checkbox.length; i++) {
        if (checkbox[i].checked) {
            boolval = true;
        }

    }

    if (boolval == false) {
        message = message + "\n" + "Please select atleast one item from Test";

    }
    if (message == "") {
        document.getElementById(lblerror).innerText = "";
        if (confirm("Do you wish to save the Test Group?"))
            return true;
        else
            return false;
    }
    else {
        document.getElementById(lblerror).innerText = message;
        return false;
    }

}

function ValidateDrugGroup(chkTest, lblerror) {
    var message = "";

    var boolval = false;
    var CHK = document.getElementById(chkTest);
    var checkbox = CHK.getElementsByTagName("input");
    var counter = 0;
    for (var i = 0; i < checkbox.length; i++) {
        if (checkbox[i].checked) {
            boolval = true;
        }

    }

    if (boolval == false) {
        message = message + "\n" + "Please select atleast one item from Test";

    }
    if (message == "") {
        document.getElementById(lblerror).innerText = "";
        if (confirm("Do you wish to save the Drug Group?"))
            return true;
        else
            return false;
    }
    else {
        document.getElementById(lblerror).innerText = message;
        return false;
    }

}

function validationForDeletion(FunctionName) {

    if (confirm("Do you wish to delete the " + FunctionName + "?"))
        return true;
    else
        return false;
}
