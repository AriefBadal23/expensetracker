import {Buckets} from "../types/Buckets.tsx";
import {IdToBucket} from "./BucketMap.ts";


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


export function validateDescription(name:string):boolean{
    // Validate if the description is not more then 50 characters and consists out of certain allowed characters.
    const regex = /^[\p{L}0-9][\p{L}0-9\s'-]{1,49}$/u;
    const result= regex.test(name)
    if(result){
        return true
    }
    return false;
}


export function validateAmount(amount:string):boolean{
    // Validate if amount is not larger then 100_000 and more then 0.
    const regex = /^(?:100000|[0-9]{1,5})$/u
    const result = regex.test(amount)
    if(result && Number(amount) > 0){
        return true
    }
    return false
}
export  function validateCreateDate(create_date:Date):boolean {
    // Validate the create_date if it is not later then one year from today.
    // TODO: Add more constraints
    const today = new Date()
    return create_date >= today && create_date != null;
}

export function validateBucketId(bucket:number){
    const bucketKeys = Object.values(Buckets) as Buckets[];
    
    const bucketExists = bucketKeys.includes(IdToBucket[bucket])
    if(!bucketExists){
        return false
    }
    return true;
}

export function UserIsLoggedIn():boolean{
    const isLoggedIn = localStorage.getItem("isLoggedIn");
    return !!isLoggedIn;
}