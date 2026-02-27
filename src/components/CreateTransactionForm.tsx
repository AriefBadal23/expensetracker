import {useEffect, useState} from "react";
import type {Transaction} from "../types/Transaction";
import {Buckets} from "../types/Buckets";
import type {NewTransactionRow} from "../types/NewTransactionRow.tsx";
import {BucketToId, IdToBucket} from "../utils/BucketMap.ts";

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
        <div className="form-floating mb-3">
          <input
            className="form-control"
            required
            type="text"
            name="description"
            onChange={(e) => change(e)}
            value={formdata.description}
          />
          <label htmlFor="name">Name</label>
        </div>
        <div className="form-floating mb-3">
          <input
            className="form-control"
            required
            type="number"
            onChange={change}
            name="amount"
            placeholder="amount"
            value={formdata.amount}
          />
          <label htmlFor="amount">Amount</label>
          
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

        <div className="form-floating mb3">
          <input
            type="date"
            className="form-control"
            name="created_at"
            onChange={change}
            value={formdata.created_at.toString()}
          />
          <label htmlFor="created_at">Date</label>
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

// Enum key/id => string naam van de bucket is
// string name van de bucket geef ik door aan updateBucketAmount()
