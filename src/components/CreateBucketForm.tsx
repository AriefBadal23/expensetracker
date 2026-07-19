import * as React from "react";
import {type Dispatch, type SetStateAction, useEffect, useState} from "react";
import {getErrorMessage, validateBucketName, validateIcon} from "../utils/utils.ts";
import {type Bucket as BucketType, BucketTypes} from "../types/Bucket";


type Bucket = {
    id?:number
    name: string
    icon: string
    type: BucketTypes
}



interface CreateBucketFormProps {
    isUpdateForm?: boolean
    // BucketType to differentiate between the Bucket Component with an as export (see top line)
    setBuckets: React.Dispatch<React.SetStateAction<BucketType[]>>
    setErrorMessage?: Dispatch<SetStateAction<Error | undefined>>
    bucketId?: number
    setIsUpdateForm?: Dispatch<SetStateAction<{isOpen: boolean, bucketId?: number}>>
    
}

const CreateBucketForm = ({isUpdateForm,setIsUpdateForm, setBuckets, setErrorMessage, bucketId}: CreateBucketFormProps) => {
    
    const [formData, setFormdata] = useState<Bucket>({
            id:bucketId,
            name: "",
            icon: "",
            type: BucketTypes.Expense
})

    
    const [errors, setErrors] = useState({ name: "", icon:"", type:"", uiMessage: ""});
    
    const canSubmit = Object.values(errors).every(value => value === "");

    useEffect(() => {
        const fetchBucketDetails = async () => {
            try {
                const response = await fetch(`https://localhost:7118/api/v1/buckets/details?id=${bucketId}`,
                    {
                        credentials: "include"
                    });

                if (!response.ok) {
                    let message = "Something went wrong."

                    if (response.status === 401) {
                        message="Unauthorized access."
                    }

                    else if (response.status === 404) {
                        message="Unable to retrieve bucket details."

                    }
                    setErrors(prev => ({
                        ...prev,
                        uiMessage: message
                    }))

                    // implicit return to stop flow
                    return;
                }

                const data = await response.json();
                console.log(data.value.bucket)
                setFormdata(data.value.bucket);



            } catch (e) {
                const message = getErrorMessage(e);
                console.error(message)
                setErrors(prev => ({
                    ...prev,
                    uiMessage: "Not able to retrieve transaction details"
                }));
            }


        }
        
        if (isUpdateForm && bucketId) {
            const init = async () => {
                await fetchBucketDetails();
            };

            init();
        }
        

    }, [isUpdateForm, bucketId]); // alleen aanroepen als deze veranderen


    const updateBucket = async () => {
        try{
            const response = await fetch(`https://localhost:7118/api/v1/buckets/${bucketId}`,{
                method: "Put",
                credentials: "include",
                body: JSON.stringify(formData),
                headers: {
                    "Content-type": "application/json; charset=UTF-8",
                },
            })

            let message = "Something went wrong."
            if(!response.ok){
                console.error("PUT /transactions failed", {
                    status: response.status,
                    statusText: response.statusText
                });
                if(response.status === 400){
                    message = "Invalid input provided."
                    console.error(response.statusText)


                }
                if(response.status === 401){
                    message =  "Unauthorized access."
                }
                else if(response.status === 404){
                    message=  "Unable to update the transaction"
                    console.error(response.statusText)


                }
                setErrors(prev => ({
                    ...prev,
                    uiMessage: message
                }))
                return;
            }

            const data = await response.json();
            console.log("Updates here....")
            console.log(data)
            
            
            if(setIsUpdateForm !== undefined){
                setIsUpdateForm({isOpen:false});
            }
            
            
            const updatedBucket: BucketType = {
                bucketTotal: data.value.bucket.bucketTotal,
                bucket: {
                    id: data.value.bucket.id,
                    name: data.value.bucket.name,
                    icon: data.value.bucket.icon,
                    type: data.value.bucket.type,
                }
                
            }

            setBuckets(prev => 
            prev.map(b => b.bucket.id === updatedBucket.bucket.id ? updatedBucket : b)
            );
            
        }
        catch(e){
            const message = getErrorMessage(e)
            console.error(message)
            setErrors(prev => ({
                ...prev,
                uiMessage: "Failed to update the transaction."
            }))
        }
    }


    function handleIconChange(value: string) {
        if(!validateIcon(value)){
            setErrors((prev) => (
                {
                    ...prev,
                    // icon is the property for the form field to hold the error message for it.
                    icon: "The icon for the bucket should only be an emoji."
                }
            ))
        }

        else{
            setErrors(prev => ({
                ...prev,
                icon: "",
                uiMessage:""

            }))
        }
    }

    function handleTypeChange(value: string) {
    const validTypes = Object.values(BucketTypes);
    if (!validTypes.includes(value as BucketTypes)) {
        setErrors(prev => ({
            ...prev,
            type: "The type must be either Income or Expense."
        }));
    } else {
        setErrors(prev => ({
            ...prev,
            type: "",
            uiMessage: ""
        }));
    }
}
    const change = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        switch(name) {
            case "name":
                handleBucketNameChange(value);
                break;
            case "icon":
                handleIconChange(value);
                break;
            case "type":
                handleTypeChange(value);
                break;
        }

        //!   wat doet [e.target.name]: e.target.value => computed property name
        setFormdata(prev => ({
            ...prev,
            [name]: value,
        }));
        
        
        
        
    }
    const handleBucketNameChange = (name:string) =>{
        if(!validateBucketName(name)){
            setErrors(prev => ({
                ...prev,
                // name is the property for the form field to hold the error message for it.
                name: "The name must be between 1 and 15 characters and contain only allowed characters",
            }))
        }
        else{
            setErrors(prev => ({
                ...prev,
                name: "",
                uiMessage:""
    
            }))
        }
        
    }
    
    const PostBucket = async () => {
        try {
            const response = await fetch("https://localhost:7118/api/v1/buckets", {
                method: "Post",
                body: JSON.stringify(formData),
                headers: {
                    "Content-type": "application/json; charset=UTF-8",
                },
                credentials: "include"

            });

            // do message state update just once.
            let message = "Something went wrong."
            if (!response.ok) {
                // log error for debug purposes.
                console.error("POST /buckets failed", {
                    status: response.status,
                    statusText: response.statusText
                });
                if (response.status === 400) {
                    message = "Invalid input provided."

                } else if (response.status === 401) {
                    message = "Unauthorized access."

                }
                // else not required.
                setErrors(prev => ({
                    ...prev,
                    uiMessage: message
                }));
                
                if(setErrorMessage){
                    setErrorMessage(new Error(message))
                }

                // early return to stop flow here.
                return;
            }

            const data = await response.json()
            console.log(`POST /buckets response`, data);

            // 💡Make sure the structure is the same as the API response.
            const newBucket: BucketType = {
                bucketTotal: data.value.bucketTotal,
                bucket: {
                    id: data.value.bucket.id,
                    name: data.value.bucket.name,
                    icon: data.value.bucket.icon,
                    type: data.value.bucket.type
                }
                
            }
            

            // Dit maakt een nieuwe array door oude values van de huidige state te kopieeren
            // naar een de nieuwe array met de nieuwe transactie.
            // Hiervoor heb ik een spread operator gebruikt. Door dit doen wordt er re-render gedaan.

            setBuckets((prev) => [...prev, newBucket ]);

        } catch (e) {
            const message = getErrorMessage(e);
            console.error(message)
            setErrors(prev => ({
                ...prev,
                uiMessage: "Not able to create new bucket."
            }));
            
            if(setErrorMessage){
                setErrorMessage(new Error("Not able to create new bucket."))
            }
        }
    }
    
    async function SubmitData(){
        if(isUpdateForm){
            try{
                await updateBucket()
                
            }
            catch (e){
                const message = getErrorMessage(e)
                console.error(message)
            }
        }
        
        else {
            try{
                await PostBucket();
            }
            catch(e){
                const message = getErrorMessage(e)
                console.error(message)
            }
        }
        if(setIsUpdateForm){
            setIsUpdateForm({isOpen:false})
        }
        // setShowModal(false)

    }
    
    return (
        <form onSubmit={async (e) => {
            
            e.preventDefault()
            await SubmitData();
            setFormdata(() => (
                {

                    name: "",
                    icon: "",
                    type: BucketTypes.Expense
                }
            ))
            
            
            }}>
            <div>
                <label htmlFor="bucket">Name: </label>
                {/* If there is an error for the bucket name in the form field show it in the UI */}
                {errors["name"] && <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["name"]}</p>}
                
                
                <input className="form-control" 
                       type="text" 
                       name="name" 
                       placeholder="Savings"
                       value={formData.name}
                       onChange={change}
                />

                <label htmlFor="bucket">Icon: </label>
                
                {errors["icon"] && <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["icon"]}</p>}
                <input
                    className="form-control" 
                    required
                    type="text" 
                    onChange= {change}
                    name="icon"
                    placeholder="e.g. 💸"
                    value={formData.icon}
                    title="Add emoji icon for bucket."
                />
                
                
               <label>Type:</label>
                <select
                  name="type"
                  className="form-control"
                  value={formData.type}
                  onChange={(e) => change(e as unknown as React.ChangeEvent<HTMLInputElement>)}
                >
                  <option value={BucketTypes.Income}>Income</option>
                  <option value={BucketTypes.Expense}>Expense</option>
                </select>
                {/* If there is an error for the bucket name in the form field show it in the UI */}
                {errors["type"] && <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["type"]}</p>}
                <input className="btn btn-primary" type="submit" value="Submit" style={{marginTop: "0.60rem"}} disabled={!canSubmit}/>
            </div>
        </form>
    )

}
export default CreateBucketForm