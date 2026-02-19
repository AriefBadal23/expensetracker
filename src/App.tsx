import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import type { Transaction } from "./types/Transaction";
import Pagination from "./components/Pagination";
import Filter from "./components/Filter";
import Overview from "./components/Overview"
import Navbar from "./components/NavBar.tsx";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);

  function UpdateTable(
    newamount: number,
    newdescription: string,
    newbucket: number,
    newcreated_at: Date,
    isIncome: boolean
  ): void {
    setTransactions((prev) => [
      ...prev,
      {
        amount: newamount,
        description: newdescription,
        bucketId: newbucket,
        created_at: newcreated_at,
        isIncome: isIncome,
      },
    ]);
  }
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route
          path="/overview"
          element={<Overview/>}
          />
          <Route
            path="/"
            element={
              <>
                <BucketList transactions={transactions} />
                <CreateFormModal updateTable={UpdateTable} isUpdateForm={false} />
                <Filter />
                <TransactionTable transactions={transactions} setTransactions={setTransactions} />
                <Pagination setTransactions={setTransactions} />
              </>
            }
          />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
