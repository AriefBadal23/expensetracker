import { useState } from "react";
import type { Transaction } from "../types/Transaction";
import type { NewTransactionRow as AddNewTransactionRow } from "../types/NewTransactionRow";
import { Buckets } from "../types/Buckets";

const CreateForm = ({
  updateTable,
  updateBucketAmount,
}: AddNewTransactionRow) => {
  const [formdata, setFormData] = useState<Transaction>({
    amount: 0,
    description: "",
    bucket: Buckets.Groceries,
  });

  const keys = Object.keys(Buckets);

  const change = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLSelectElement>
  ) => {
    //!   wat doet [e.target.name]: e.target.value
    setFormData({ ...formdata, [e.target.name]: e.target.value });
  };


  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          updateTable(formdata.amount, formdata.description, formdata.bucket);
          updateBucketAmount(formdata.bucket, formdata.amount);
          // bucket as keyof typeof Buckets
        }}
      >
        
        <label>Name</label>
        <input
          required
          type="text"
          name="description"
          onChange={(e) => change(e)}
          value={formdata.description}
        />
        <label>Amount</label>
        <input
          required
          type="number"
          onChange={change}
          name="amount"
          value={formdata.amount}
        />
        <label>Bucket</label>
        <select required name="bucket" onChange={(e) => change(e)}>
          <option>Choose a bucket</option>
          {keys.map((key) => {
            return <option value={key}>{key}</option>;
          })}
        </select>
        <input type="submit" onChange={(e) => change(e)} value="Send" />
      </form>
    </>
  );
};

export default CreateForm;

// Enum key/id => string naam van de bucket is
// string name van de bucket geef ik door aan updateBucketAmount()
