import { useState } from "react";
import type { Transaction } from "../types/Transaction";
import type { NewTransactionRow as AddNewTransactionRow } from "../types/NewTransactionRow";

const CreateForm = ({ updateTable }: AddNewTransactionRow) => {
  const [formdata, setFormData] = useState<Transaction>({
    amount: 0,
    description: "",
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
          updateTable(formdata.amount, formdata.description);
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
        <input type="submit" onChange={(e) => change(e)} value="Send" />
      </form>
    </>
  );
};

export default CreateForm;
