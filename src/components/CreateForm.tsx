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

  const change = (e: React.ChangeEvent<HTMLInputElement>) => {
    //!   wat doet [e.target.name]: e.target.value
    setFormData({ ...formdata, [e.target.name]: e.target.value });
  };

  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          updateTable(formdata.amount, formdata.description, formdata.bucket);
          updateBucketAmount(1, formdata.amount);
          console.log("Executed!");
        }}
      >
        <label>Name</label>
        <input
          type="text"
          name="description"
          onChange={(e) => change(e)}
          value={formdata.description}
        />
        <label>Amount</label>
        <input
          type="number"
          onChange={change}
          name="amount"
          value={formdata.amount}
        />

        <label>Bucket</label>
        <input
          type="text"
          onChange={change}
          name="bucket"
          value={formdata.bucket}
        />
        <input type="submit" onChange={(e) => change(e)} value="Send" />
      </form>
    </>
  );
};

export default CreateForm;
