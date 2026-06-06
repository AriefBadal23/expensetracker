import * as React from "react";
import {type Dispatch, type SetStateAction, useState} from "react";
import {getErrorMessage, validateBucketName, validateIcon} from "../utils/utils.ts";
import {type Bucket as BucketType, BucketTypes} from "../types/Bucket";


type Bucket = {
    name: string
    icon: string
    type: BucketTypes
}



interface CreateBucketFormProps {
    setShowModal: React.Dispatch<React.SetStateAction<boolean>>
    setBuckets: React.Dispatch<React.SetStateAction<BucketType[]>>
    setErrorMessage: Dispatch<SetStateAction<Error | undefined>>
    
}

const CreateBucketForm = ({setShowModal, setBuckets, setErrorMessage}: CreateBucketFormProps) => {
    
    const [formData, setFormdata] = useState<Bucket>({
            name: "",
            icon: "",
            type: BucketTypes.Expense
})

    
    const [errors, setErrors] = useState({ name: "", icon:"", uiMessage: ""});
    
    const canSubmit = Object.values(errors).every(value => value === "");

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

    const change = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        switch(name) {
            case "name":
                handleBucketNameChange(value);
                break;
            case "icon":
                handleIconChange(value);
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
                
                setErrorMessage(new Error(message))

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
            setErrorMessage(new Error("Not able to create new bucket."))
        }
    }
    
    return (
        <form onSubmit={async (e) => {
            e.preventDefault()
            await PostBucket();

            setFormdata(() => (
                {
                    
                        name: "",
                        icon: "",
                        type: BucketTypes.Expense                 
                }
            ))
            
            setShowModal(false)
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
                {/* If there is an error for the bucket name in the form field show it in the UI */}
                {errors["icon"] && <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["icon"]}</p>}
                
                <input className="btn btn-primary" type="submit" value="Submit" style={{marginTop: "0.60rem"}} disabled={!canSubmit}/>
            </div>
        </form>
    )

}
export default CreateBucketForm