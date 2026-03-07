

export function getErrorMessage(error:unknown):string{
    let message:string;
    if(error instanceof Error){
        message = error.message;
    }
    else if(error && typeof error === 'object' && 'message' in error){
        message = String(error.message)
    }
    else if(typeof error === 'string'){
        message = error;
    }
    else {
        message = "Something went wrong"
    }
    return message;
}


export function validateNameValue(name:string):boolean{
    const regex = /^[\p{L}0-9][\p{L}0-9\s'-]{1,49}$/u;
    const result= regex.test(name)
    if(name.length >= 5 && result){
        return true;
    }
    return false;
}


export function validateAmount(amount:string):boolean{
    const regex = /^(?:100000|[0-9]{1,5})$/u
    const result = regex.test(amount)
    if(result && amount.length > 0){
        return true
    }
    return false
}

export function validateCreatedDate(date:Date){
    const dateRegex = /^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$/;
    const result = dateRegex.test(date.toString())
    if(result){
        return true
    }
    return false;
}



//*
// 01-02-2025 %
// 01-03-2026 X
// */
export function isValidDateRange(form_createdate:Date):boolean {

    
}