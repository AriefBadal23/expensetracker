import { useState } from "react";
import type { Transaction } from "../types/Transaction";
import type { NewTransactionRow as AddNewTransactionRow } from "../types/NewTransactionRow";
import { Buckets } from "../types/Buckets";
import { BucketToId } from "../utils/BucketMap";

const CreateTransactionForm = ({ updateTable }: AddNewTransactionRow) => {
  const [formdata, setFormData] = useState<Transaction>({
    bucketId: 0,
    userId: 1,
    description: "",
    amount: 0,
    created_at: new Date(),
  });

  // 💡 force keys to be enum values
  const bucketKeys = Object.values(Buckets) as Buckets[];

  function SubmitData() {
    fetch("http://localhost:5286/api/v1/transactions", {
      method: "Post",
      body: JSON.stringify(formdata),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    });
  }
  const change = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLSelectElement>
  ) => {
    //!   wat doet [e.target.name]: e.target.value
    setFormData({ ...formdata, [e.target.name]: e.target.value });
    console.log(`BucketId gekozen:${formdata.bucketId}`);
  };

  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          updateTable(formdata.amount, formdata.description, formdata.bucketId);
          SubmitData();

          // clear form after submit
          setFormData({
            amount: 0,
            bucketId: 0,
            description: "",
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
            value={formdata.amount}
            placeholder="amount"
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
            <option selected>Choose a bucket</option>
            {bucketKeys.map((key) => {
              return <option value={BucketToId[key]}>{key}</option>;
            })}
          </select>
          <label htmlFor="bucketId">Bucket</label>
        </div>

        <input
          type="submit"
          onChange={(e) => change(e)}
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
