import {type Dispatch, type SetStateAction, useEffect, useState} from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import "../styles/Filter.css";
import type {Bucket} from "../types/Bucket.tsx";

interface FilterProps {
    buckets: Bucket[]
    setCurrentPage: Dispatch<SetStateAction<number>>
}


const Filter = ({buckets, setCurrentPage}: FilterProps) => {
    const [isShown, setisShown] = useState<boolean>(false);
    const [errorMessage, setErrorMessage] = useState<Error | undefined>();
    const [userBuckets, setUserBuckets] = useState<Bucket[]>([]);

    const [search] = useSearchParams();
    const activeId = search.get("id");
    const navigate = useNavigate();


    useEffect(() => {
        const fetchUserbuckets = async () => {
            const URL = "https://localhost:7118/api/v1/buckets/user"
            try {
                const response = await fetch(URL, {
                    method: "GET",
                    credentials: "include"
                })
                if (!response.ok) {
                    let message = "Something went wrong."
                    console.error("GET /buckets/user failed", {
                        status: response.status,
                        statusText: response.statusText
                    });

                    if (response.status === 401) {
                        message = "Unauthorized access"
                    }
                    setErrorMessage(new Error(message))
                    return;
                }
                const data = await response.json();
                setUserBuckets(data.value)

            } catch (e) {
                console.error(e)
            }
        }
        fetchUserbuckets()
    }, [buckets])

    useEffect(() => {
        setCurrentPage(1);
    }, [activeId, setCurrentPage]);
    
    

    const ErrorMessageStyle = {
        color: "#B00020",
        backgroundColor: "#FFEBEE",
        borderLeft: "4px solid #D32F2F",
        padding: "8px 12px",
        borderRadius: "4px",
        fontSize: "16px",
        lineHeight: "1.4",
        fontFamily: "Segoe UI, Tahoma, sans-serif",
        marginTop: "6px"
    };

    const CURRENT_YEAR = 2026
    
    return (
        <div
            id="transaction-filter"
            className="btn-group"
            role="group"
            aria-label="Transaction filter"
        >
            {errorMessage && <div><p style={ErrorMessageStyle}>{errorMessage.message}</p></div>}
            <input
                type="radio"
                className="btn-check"
                name="bucket"
                id="bucket-0"
                checked={activeId === null}
                onChange={() => {
                    navigate("/dashboard");
                    setisShown(false);
                }}
            />
            <label className="btn btn-outline-primary" htmlFor="bucket-0">
                All buckets
            </label>
            {
                userBuckets.map((bucket: Bucket) => (
                    <div key={bucket.bucket.id}>
                        <input
                            type="radio"
                            className="btn-check"
                            name={bucket.bucket.name}
                            id={bucket.bucket.name}
                            checked={activeId === bucket.bucket.id.toString()}
                            onChange={() => {
                                navigate(`?id=${bucket.bucket.id}&year=${CURRENT_YEAR}`);
                                setisShown(false);
                            }}
                        />
                        <label className="btn btn-outline-primary" htmlFor={bucket.bucket.name}>
                            {bucket.bucket.name}
                        </label>


                    </div>
                ))
            }
            <input
                type="radio"
                className="btn-check"
                name="filter"
                id="filter"
                value="Filter on month"
                checked={activeId === "filter"}
                onClick={() => setisShown(!isShown)}
            />

        </div>
    );
};

export default Filter;