import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import BucketDetail from "./components/BucketDetail";
import type { Transaction } from "./types/Transaction";
import Pagination from "./components/Pagination";
import Filter from "./components/Filter";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);

  function UpdateTable(
    newamount: number,
    newdescription: string,
    newbucket: number,
    newcreated_at: string,
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
            path="/transactions"
            element={
              <>
                <Filter />
                <BucketList />
                <CreateFormModal updateTable={UpdateTable} />
                <TransactionTable transactions={transactions} />
                <Pagination setTransactions={setTransactions} />
              </>
            }
          />
          {/* <Route path="transactions/:name" element={<BucketDetail />} /> */} 
          {/* bucket detail is unneccasry now because the filtering can be done at the home page */}
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
