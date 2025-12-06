import { useState } from "react";
import type { Transaction } from "../types/Transaction";
import type { NewTransactionRow as AddNewTransactionRow } from "../types/NewTransactionRow";
import { Buckets } from "../types/Buckets";

const CreateTransactionForm = ({
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

          // clear form after submit
          setFormData({
            amount: 0,
            bucket: Buckets.Groceries,
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
            name="bucket"
            onChange={(e) => change(e)}
          >
            <option selected>Choose a bucket</option>
            {keys.map((key) => {
              return <option value={key}>{key}</option>;
            })}
          </select>
          <label htmlFor="bucket">Bucket</label>
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
