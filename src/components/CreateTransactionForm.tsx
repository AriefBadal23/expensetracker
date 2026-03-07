import {useEffect, useState} from "react";
import type {Transaction} from "../types/Transaction";
import {Buckets} from "../types/Buckets";
import type {NewTransactionRow} from "../types/NewTransactionRow.tsx";
import {BucketToId, IdToBucket} from "../utils/BucketMap.ts";
import {isValidDateRange, validateAmount, validateCreatedDate, validateNameValue} from "../utils/utils.ts"; // named export

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
  
  const [errors, setErrors] = useState({description: "", amount: "", created_at:""})

  useEffect(() => {
    if (isUpdateForm && transactionID) {
      const fetchData = async () => {
        try {
          const response = await fetch(`http://localhost:5286/api/v1/transactions/details?id=${transactionID}`);
          const data = await response.json();
          setFormData(data);
        } catch (e) {
          console.log(e);
        }
      };
      fetchData();
    }
  }, [isUpdateForm, transactionID]); // alleen aanroepen als deze veranderen
  

  // 💡 force keys to be enum values
  const bucketKeys = Object.values(Buckets) as Buckets[];
  
  
    const handleCreationDateChange = () => {
        const isValid = validateCreatedDate(formdata.created_at) && isValidDateRange(formdata.created_at)
        console.log(isValid)
        if(isValid){
            setErrors(prev => ({
                ...prev,
                created_at : "Incorrect date for new transaction"
            }))
        }
    }
    
    const handleDescriptionChange = () => {
      if(!validateNameValue(formdata.description)){
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
    
    const handleAmountChange = () => {
    if (!validateAmount(formdata.amount.toString())) {
      setErrors(prev => ({
        ...prev,
        amount: "Incorrect amount"
      }))
    } else {
      setErrors(prev => ({
        ...prev,
        amount: ""
      }))
    }
    }

  async function SubmitData() {
    if(isUpdateForm){
      try{
           await fetch("http://localhost:5286/api/v1/transactions",{
           method: "Put",
           body: JSON.stringify(formdata),
           headers: {
             "Content-type": "application/json; charset=UTF-8",
           },
         })
       }
      catch(e){
        console.log(e)
      }
     }
    else{
      try{
        const response = await fetch("http://localhost:5286/api/v1/transactions", {
          method: "Post",
          body: JSON.stringify(formdata),
          headers: {
            "Content-type": "application/json; charset=UTF-8",
          },
        });
        
        const data = await response.json()

        const newTransaction: Transaction = {
          id: data.id,
          bucketId: data.bucketId,
          userId: data.userId,
          description: data.description,
          amount: data.amount,
          created_at: new Date(data.created_at), 
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
    
    handleDescriptionChange()
    handleAmountChange()
    handleCreationDateChange()
    
    
    
    //!   wat doet [e.target.name]: e.target.value
    setFormData(() => (
        {
          ...formdata,
          [e.target.name]:
          e.target.value,
        }
    ));
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
            style={{
              borderRadius: "5px",
              border: "1px solid #ced4da",
              boxShadow: errors["description"]
                  ? "0 0 5px rgba(220, 53, 69, 0.5)" 
                  : "none",
              transition: "box-shadow 0.2s, border 0.2s"
            }}
        >
          <input
              className="form-control"
              required
              type="text"
              name="description"
              value={formdata.description}
              onChange={change}
              style={{
                border: "none", 
                boxShadow: "none"
              }}
              pattern="[A-Za-z0-9][A-Za-z0-9\s'-]{1,49}"
              title="Beschrijving moet beginnen met een letter of cijfer, 1-50 tekens lang, alleen letters, cijfers, spaties, apostrof en streepje toegestaan"
          />
          <label htmlFor="name">Name</label>
          {errors["description"] && (
              <p style={{ color: "red", marginTop: "0.25rem" }}>{errors["description"]}</p>
          )}
        </div>  
        <div
            className="form-floating mb-3"
            style={{
              borderRadius: "5px", 
              border: "1px solid #ced4da",
              boxShadow: errors["amount"]
                  ? "0 0 5px rgba(220, 53, 69, 0.5)" 
                  : "none",
              transition: "box-shadow 0.2s, border 0.2s"
            }}
        >
          <input
              className="form-control"
              required
              type="text"
              onChange={change}
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

        <div className="form-floating mb-3">
          <select
            className="form-select"
            required
            name="bucketId"
            onChange={(e) => change(e)}
          >
            {isUpdateForm ? <option>{IdToBucket[formdata.bucketId]}</option> : <option value={0}>Choose a bucket</option>}
            {bucketKeys.map((key) => {
              return <option key={key} value={BucketToId[key]}>{key}</option>;
            })}
          </select>
          <label htmlFor="bucketId">Bucket</label>
        </div>

        <div className="form-floating mb-3">
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

// Enum key/id => string naam van de bucket
