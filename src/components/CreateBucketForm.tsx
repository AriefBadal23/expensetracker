import * as React from "react";
import {useState} from "react";
import {getErrorMessage} from "../utils/utils.ts";
import {type Bucket as BucketType, BucketTypes} from "../types/Bucket";


type Bucket = {
    name: string
    icon: string
    type: BucketTypes
}



interface CreateBucketFormProps {
    setShowModal: React.Dispatch<React.SetStateAction<boolean>>
    setBuckets: React.Dispatch<React.SetStateAction<BucketType[]>>
}

const CreateBucketForm = ({setShowModal, setBuckets}: CreateBucketFormProps) => {
    const [formData, setFormdata] = useState<Bucket>({
            name: "",
            icon: "",
            type: BucketTypes.Expense
})


    const [errors, setErrors] = useState([]);

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
                <input type="text" name="bucket" id="name" value={formData.name}
                       onChange={(e) => setFormdata(prevState => (
                           {
                               ...prevState,
                               name: e.target.value,
                           }
                       ))}
                />
                <label htmlFor="bucket">Icon: </label>
                <input type="text" name="icon" id="icon" value={formData.icon}
                       onChange= {(e) => (
                           setFormdata((prevState) => (
                               {
                                    ...prevState,
                                   icon: e.target.value,
                               }
                           ))
                       ) }
                />


                <input className="btn btn-primary" type="submit" value="Submit"/>
            </div>
        </form>
    )

}
export default CreateBucketForm