import {useEffect, useState} from "react";
import type {Transaction} from "../types/Transaction";
import {Buckets} from "../types/Buckets";
import type {NewTransactionRow} from "../types/NewTransactionRow.tsx";
import {BucketToId, IdToBucket} from "../utils/BucketMap.ts";

const CreateTransactionForm = ({ updateTable,isUpdateForm, transactionID, SetShowModal, showModal }: NewTransactionRow) => {
  
  // NOTE: Voor een transaction is het niet nodig om een ID mee te geven. 
  // Dit omdat EFC en PostgreSQL een auto-incremented ID aanmaken.
  
  
  const [formdata, setFormData] = useState<Transaction>({
    bucketId: 0,
    userId: 1,
    description: "",
    amount: 0,
    created_at: new Date(),
    isIncome: false,
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
  
  
  
  function SubmitData() {
    if(isUpdateForm){
       try{
         fetch("http://localhost:5286/api/v1/transactions",{
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
        fetch("http://localhost:5286/api/v1/transactions", {
          method: "Post",
          body: JSON.stringify(formdata),
          headers: {
            "Content-type": "application/json; charset=UTF-8",
          },
        });
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
    setFormData({
      ...formdata,
      [e.target.name]:
        e.target.type === "checkbox" ? e.target.checked : e.target.value,
    });
  };
  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          const created_atDate = new Date(formdata.created_at);
          
          if(showModal === true && SetShowModal !== undefined){
            SetShowModal(false)
          }
          
          if(updateTable !== undefined){
            updateTable(
              formdata.amount,
              formdata.description,
              formdata.bucketId,
              created_atDate,
              formdata.isIncome
            )
          }
          
            

          SubmitData();

          // clear form after submit
          setFormData({
            amount: 0,
            bucketId: 0,
            description: "",
            created_at: "",
            isIncome: false,
          });
        }}
      >
        <div className="form-check mb-3">
          <input
            className="form-check-input"
            type="checkbox"
            name="isIncome"
            checked={IdToBucket[formdata.bucketId] === Buckets.Salary ? true : formdata.isIncome}
            onChange={(e) => {
              change(e);
            }}
            
          />
          <label className="form-check-label" htmlFor="flexCheckChecked">
            This transaction is an Income.
          </label>
        </div>

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
            {isUpdateForm ? <option selected>{IdToBucket[formdata.bucketId]}</option> : <option selected>Choose a bucket</option>}
            {bucketKeys.map((key) => {
              return <option value={BucketToId[key]}>{key}</option>;
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
            value={formdata.created_at}
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
