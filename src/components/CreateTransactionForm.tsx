import {useEffect, useState} from "react";
import type {Transaction} from "../types/Transaction";
import {Buckets} from "../types/Buckets";
import type {NewTransactionRow} from "../types/NewTransactionRow.tsx";
import {BucketToId, IdToBucket} from "../utils/BucketMap.ts";
import {validateCreateDate, validateAmount, validateDescription, validateBucketId} from "../utils/utils.ts"; // named export

const CreateTransactionForm = ({isUpdateForm, transactionID, SetShowModal, showModal, setTransactions }: NewTransactionRow) => {
  
  // NOTE: Voor een transaction is het niet nodig om een ID mee te geven. 
  // Dit omdat EFC en PostgreSQL een auto-incremented ID aanmaken.
  const [formdata, setFormData] = useState<Transaction>({
    bucketId: 0,
    userId: 1,
    description: "",
    amount: 0,
    created_at: new Date(),
  });
  
  const [errors, setErrors] = useState({description: "", amount: "", created_at:"", bucket_id: ""})
    
  const canSubmit = Object.values(errors).every(value => value === "");
  const errorStyle = {
        borderRadius: "5px",
            border: "1px solid #ced4da",
            boxShadow: errors["description"]
            ? "0 0 5px rgba(220, 53, 69, 0.5)"
            : "none",
            transition: "box-shadow 0.2s, border 0.2s"
    }
  useEffect(() => {
    if (isUpdateForm && transactionID) {
      const fetchData = async () => {
        try {
          const response = await fetch(`https://localhost:7118/api/v1/transactions/details?id=${transactionID}`, 
          {
              credentials: "include"
          });
          const data = await response.json();
          setFormData(data.value);
        } catch (e) {
          console.log(e);
        }
      };
      fetchData();
    }
  }, [isUpdateForm, transactionID]); // alleen aanroepen als deze veranderen
    
  // 💡 force keys to be enum values
  const bucketKeys = Object.values(Buckets) as Buckets[];
  
    const handleCreationDateChange =  (date:string) => {
        const isValid =   validateCreateDate(new Date(date))
        if(isValid){
            setErrors(prev => ({
                ...prev,
                created_at : "Incorrect date for new transaction"
            }))
        }
        else{
            setErrors(prev => ({
                ...prev,
                created_at : ""
            }))
        }
    }
    
    const handleDescriptionChange = (description:string) => {
        
      if(!validateDescription(description)){
        setErrors(prev => ({
            ...prev,
            description : "Incorrect name for transaction"
        }))
        
      }
      else{
        setErrors(prev => ({
          ...prev,
          description : ""
        }))
    }
    }
    
    const handleAmountChange = (amount:string) => {
    if (!validateAmount(amount)) {
      setErrors(prev => ({
        ...prev,
        amount: "Amount should be more then 0 and less then 100.000"
      }))
    } 
    else {
      setErrors(prev => ({
        ...prev,
        amount: ""
      }))
    }
    }
    
    const handleBucketIdChange = (bucket:number) => {
        if (!validateBucketId(bucket)) {
            setErrors(prev => ({
                ...prev,
                bucket_id: "Invalid bucket"
            }))
        } else {
            setErrors(prev => ({
                ...prev,
                bucket_id: ""
            }))
        }
    }

  async function SubmitData() {
    if(isUpdateForm){
      try{
           const response = await fetch("https://localhost:718/api/v1/transactions",{
           method: "Put",
           body: JSON.stringify(formdata),
           headers: {
             "Content-type": "application/json; charset=UTF-8",
           },
           credentials: "include"
         })
       }
      catch(e){
        console.log(e)
      }
     }
    else{
      try{
        const response = await fetch("https://localhost:7118/api/v1/transactions", {
          method: "Post",
          body: JSON.stringify(formdata),
          headers: {
            "Content-type": "application/json; charset=UTF-8",
          },
            credentials: "include"
            
        });
        
        const data = await response.json()

        const newTransaction: Transaction = {
          id: data.value.id,
          bucketId: data.value.bucketId,
          userId: data.value.userId,
          description: data.value.description,
          amount: data.value.amount,
          created_at: new Date(data.value.created_at), 
        };



        // Dit maakt een nieuwe array door oude values van de huidige state te kopieeren
        // naar een de nieuwe array met de nieuwe transactie.
        // Hiervoor heb ik een spread operator gebruikt. Door dit doen wordt er re-render gedaan.
          
        setTransactions((prev) => [newTransaction, ...prev]);
        
      }
      catch(e){
        console.log(e)
      }
    
    }
  }

    

    const change = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLSelectElement>
  ) => {

      const { name, value } = e.target;

      switch(name) {
          case "amount":
              handleAmountChange(value);
              break;
          case "description":
              handleDescriptionChange(value);
              break;
          case "created_at":
              handleCreationDateChange(value);
              break;
          case "bucketId":
              handleBucketIdChange(Number(value))
      }
      
      
    
    //!   wat doet [e.target.name]: e.target.value => computed property name
      setFormData(prev => ({
          ...prev,
          [name]: value,
      }));
  };
  
  
  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          
          if(showModal === true && SetShowModal !== undefined){
            SetShowModal(false)
          }
          
          SubmitData();

          // clear form after submit
          setFormData({
            amount: 0,
            bucketId: 0,
            description: "",
            created_at: new Date(),
          });
        }}
      >
        <div
            className="form-floating mb-3"
            style={errorStyle}
        >
          <input
              className="form-control"
              required
              type="text"
              name="description"
              value={formdata.description}
              onChange={(e) => {
                  change(e)
              }}
              style={{
                border: "none", 
                boxShadow: "none"
              }}
              title="Beschrijving moet beginnen met een letter of cijfer, 1-50 tekens lang, alleen letters, cijfers, spaties, apostrof en streepje toegestaan"
          />
          <label htmlFor="name">Name</label>
          {errors["description"] && (
              <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["description"]}</p>
          )}
        </div>  
        <div
            className="form-floating mb-3"
            style={errorStyle}
        >
          <input
              className="form-control"
              required
              type="number"
              onChange={(e) => {
                  change(e)
              }}
              name="amount"
              placeholder="amount"
              value={formdata.amount}
              pattern="100000|[0-9]{1,5}"
              title="Voer een bedrag in van 0 tot 100000"
              style={{
                border: "none", 
                boxShadow: "none"
              }}
          />
          <label htmlFor="amount">Amount</label>
          {errors["amount"] && <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["amount"]}</p>}
        </div>

        <div className="form-floating mb-3" style={errorStyle}>
          <select
            className="form-select"
            required
            name="bucketId"
            title = "Select an bucket for the transaction"
            onChange={(e) => change(e)}
            style={{
                    border: "none",
                    boxShadow: "none"
                }}
          >
            {isUpdateForm ? <option>{IdToBucket[formdata.bucketId]}</option> : <option value={0}>Choose a bucket</option>}
            {bucketKeys.map((key) => {
              return <option key={key} value={BucketToId[key]}>{key}</option>;
            })}
          </select>
          <label htmlFor="bucketId">Bucket</label>
            {errors["bucket_id"] && (
                <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["bucket_id"]}</p>
            )}
        </div>

          
          
        <div className="form-floating mb-3"
             style={errorStyle}
        >
            {/*  No pattern for this input field used because of the date attribute which already takes care of the corect YYYY-MM-DD format.*/}
          <input
            type="date"
            className="form-control"
            name="created_at"
            onChange={change}
            value={formdata.created_at.toString()}
            required
            style={{
                border: "none",
                boxShadow: "none"
            }}
          />
          <label htmlFor="created_at">Date</label>
            {errors["created_at"] && (
                <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["created_at"]}</p>
            )}
        </div>

        <input
          type="submit"
          disabled={!canSubmit}
          onChange={(e) => {
            change(e)
          }
        }
          value="Save transaction"
          className="btn btn-primary"   
        />
      </form>
    </>
  );
};

export default CreateTransactionForm;

// Enum key/id → string naam van de bucket
