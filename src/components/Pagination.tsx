import {useEffect, useState} from "react";
import type { Transaction } from "../types/Transaction";
import { useSearchParams } from "react-router-dom";
import type { Dispatch, SetStateAction } from "react";
import {getErrorMessage} from "../utils/utils.ts";


interface PaginationProps {
    // Type for useState setter function is Dispatch<SetStateAction>
    setTransactions: Dispatch<SetStateAction<Transaction[]>>
    setErrorMessage: Dispatch<SetStateAction<Error | undefined>>
    setCurrentPage: Dispatch<SetStateAction<number>>
    currentPage: number
}

const Pagination = ({setTransactions, setErrorMessage, setCurrentPage, currentPage}: PaginationProps) => {
    const [total, setTotal] = useState<number>(1);

  const [search] = useSearchParams();
  const PAGESIZE = 10;
  const TOTALPAGES = total / PAGESIZE;
    const PageAmount: number = Math.ceil(TOTALPAGES);


    useEffect(() => {
    const fetchTransactions = async () => {
      try {
          const month = search.get("month")
          const year = search.get("year") 
          const bucketId = search.get("id")

          let url = `https://localhost:7118/api/v1/transactions?pageNumber=${currentPage}&pageSize=${PAGESIZE}`
          
          if(year !== null ){
              url = url + `&year=${year}`
          }
          if(month !== null ){
              url = url + `&month=${month}` 
          }
          if(bucketId !== null){
              url = url + `&bucket=${bucketId}`
          }
          
          const response = await fetch(url, {
              credentials: 'include'
              
          });
          if(!response.ok){
              let message = "Something went wrong."
              console.error("GET /transactions failed", {
                  status: response.status,
                  statusText: response.statusText
              });
              
              if(response.status === 401){
                  message  = "Unauthorized access"
              }
              setErrorMessage(new Error(message))
              return;
          }
          const data = await response.json();

          if(Array.isArray(data.value.transactions)){
            setTransactions(data.value.transactions);
            setTotal(data.value.total)
            return;
          }
          setErrorMessage(new Error("invalid type of fetched data"));
          
        
      } catch (err) {
          // 1. Log the actual error to the console.
          // 2. Show an generic message in the UI for the user.
          const message = getErrorMessage(err)
          setErrorMessage(new Error("Failed to fetch transactions data"))
          console.error(message);
        }
      
    };
    fetchTransactions();
    }, [currentPage, search, setErrorMessage, setTransactions, setTotal]); //! beide als dependancy

  return (
      <div>
          {
              Math.ceil(TOTALPAGES) === 0 ? <p>No transactions found</p> : 
                  <div>

                      <p>
                          Showing {((currentPage - 1) * PAGESIZE) + 1}-{Math.min(currentPage * PAGESIZE, total)} of {total} transactions
                      </p>

                      {/*// previous button*/}
                      <input
                          type="button"
                          value="Prev"
                          disabled={currentPage === 1}
                          onClick={() => {
                              if (currentPage > 0) {
                                  setCurrentPage(currentPage - 1);
                              }
                          }}
                      />
                      {/*// next button*/}
                      <input
                          type="button"
                          value="Next"
                          disabled={currentPage === PageAmount} // why does it work only with === (loose/strict equality in JS?)
                          onClick={() => {
                              if (currentPage <= TOTALPAGES) {
                                  setCurrentPage(currentPage + 1);
                              }
                          }}
                      />
                  </div>
          }
      </div>
  );
};

export default Pagination;
