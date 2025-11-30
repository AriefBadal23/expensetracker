import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateForm from "./components/CreateForm";

function App() {
  const [transactions, setTransactions] = useState([
    {
      description: "Monthly Subscription",
      amount: 49.99,
    },
    {
      description: "Groceries",
      amount: 87.45,
    },
  ]);

  function UpdateTable(newamount: number, newdescription: string): void {
    setTransactions((prev) => [
      ...prev,
      {
        amount: newamount,
        description: newdescription,
      },
    ]);
  }
  return (
    <>
      <BucketList />
      <TransactionTable transactions={transactions} />
      <CreateForm updateTable={UpdateTable} />
    </>
  );
}

export default App;
