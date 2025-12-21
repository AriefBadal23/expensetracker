import { useEffect, useState } from "react";
import type { Transaction } from "../types/Transaction";

interface PaginationProps {
  setTransactions: (transactions: Transaction[]) => void;
}

const Pagination = ({ setTransactions }: PaginationProps) => {
  const [page, SetPage] = useState(1);
  const [error, setError] = useState<string>();
  const [total, setTotal] = useState<number>(1);

  const PAGESIZE = 5;
  const TOTALPAGES = total / PAGESIZE;
  useEffect(() => {
    const fetchTransactions = async () => {
      try {
        const response = await fetch(
          `http://localhost:5286/api/v1/transactions?pageNumber=${page}&pageSize=${PAGESIZE}`
        );

        const data = await response.json();
        setTotal(data["total"]);
        setTransactions(data["transactions"]);
      } catch (error) {
        if (error instanceof Error) {
          setError(error.message);
        } else {
          setError("Something went wrong");
          console.log(error);
        }
      }
    };
    fetchTransactions();
  }, [page, setTransactions]); //! beide als dependancy

  return (
    <div>
      <input
        type="button"
        value="Prev"
        disabled={page === 0}
        onClick={() => {
          if (page > 0) {
            SetPage(page - 1);
          }
        }}
      />
      <input
        type="button"
        value="Next"
        disabled={page === TOTALPAGES}
        onClick={() => {
          if (page < TOTALPAGES) {
            SetPage(page + 1);
          }
        }}
      />
    </div>
  );
};

export default Pagination;
