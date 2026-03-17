import { useEffect, useState } from "react";
import type { Transaction } from "../types/Transaction";
import { useSearchParams } from "react-router-dom";
import "react-day-picker/dist/style.css";
import type { Dispatch, SetStateAction } from "react";
import {getErrorMessage} from "../utils/utils.ts";


interface PaginationProps {
    // Type for useState setter function is Dispatch<SetStateAction>
    setTransactions: Dispatch<SetStateAction<Transaction[]>>
    setErrorMessage: Dispatch<SetStateAction<Error | undefined>>
}

const Pagination = ({ setTransactions, setErrorMessage }: PaginationProps) => {
  const [page, SetPage] = useState(1);
  const [total, setTotal] = useState<number>(1);

  const [search] = useSearchParams();

  const PAGESIZE = 10;
  const TOTALPAGES = total / PAGESIZE;

  useEffect(() => {
    const fetchTransactions = async () => {
      try {
          const month = search.get("month")
          const year = search.get("year") 
          const bucketId = search.get("id") 
          
          let url = `http://localhost:5286/api/v1/transactions?pageNumber=${page}&pageSize=${PAGESIZE}`
          
          if(year !== null ){
              url = url + `&year=${year}`
          }
          if(month !== null ){
              url = url + `&month=${month}`
          }
          if(bucketId !== null){
              url = url + `&bucket=${bucketId}`
          }
          
          
          const response = await fetch(url);
          if(!response.ok){
              setErrorMessage(new Error("Failed to fetch from endpoint"))
          }
          const data = await response.json();
          
          setTotal(data["total"]);
          setTransactions(data["transactions"]);
        
      } catch (err) {
          // 1. Log the actual error to the console.
          // 2. Show an generic message in the UI for the user.
          const message = getErrorMessage(err)
          setErrorMessage(new Error("Failed to fetch transactions data"))
          console.error(message);
        }
      
    };
    fetchTransactions();
  }, [page, search, setErrorMessage, setTransactions]); //! beide als dependancy

  return (
      <>
          
        <div>
          <p>
            Page: {page}/{Math.ceil(TOTALPAGES)}
          </p>
          <input
            type="button"
            value="Prev"
            disabled={page === 1}
            onClick={() => {
              if (page > 0) {
                SetPage(page - 1);
              }
            }}
          />
          <input
            type="button"
            value="Next"
            disabled={page === Math.ceil(TOTALPAGES)} // why does it work only with === (loose/strict equality in JS?)
            onClick={() => {
              if (page <= TOTALPAGES) {
                SetPage(page + 1);
              }
            }}
          />
        </div>
      </>
  );
};

export default Pagination;
