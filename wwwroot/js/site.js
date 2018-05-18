

var validate = function() {

    var cardOwner = document.getElementById('cardOwner');
    //var ccv = document.getElementById('ccInput');
    var ccvInput = document.getElementById("myField");
    var cardNo = document.getElementById('cardNo');

    // Validate Card Owner
    var validCardOwner = false;
        if (cardOwner.value &&
            cardOwner.value !== "") {
            validCardOwner = true;   
    }

    // Validate CCV Number
    var validCCVNo = false;
        if (ccvInput.value &&
        ccvInput.value !== "") {
        validCCVNo = true;
    }

    // Validate Card Number
        var validCardNo = false;
        if (cardNo.value &&
        cardNo.value !== "") {
        validCardNo = true;
    }


    if (validCardOwner && validCCVNo & validCardNo) {
        document.getElementById('submit').click();
    } else {
        alert("Please verify your credit card infomation is correct");
    }


        
}