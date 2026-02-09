import {useEffect, useState} from "react"
import Chart from "chart.js/auto";
import { CategoryScale } from "chart.js";
import { Bar } from "react-chartjs-2";
import type {ChartBucketTransaction} from "../types/ChartBucketTransaction"
import { IdToBucket } from "../utils/BucketMap";

Chart.register(CategoryScale);


const ExpenseChart = () => {
    const [chartData, setChartData] = useState<ChartBucketTransaction>();

    useEffect(() => {
        const fetchData = async () => {
        try {
            const response = await fetch(`http://localhost:5286/api/v1/Buckets/Groceries?year=2025`)
            const transactions = await response.json()
            
            // Ensure totals are numbers (important for Chart.js)
        const normalized = {
            ...transactions,
            totals: (transactions.totals ?? []).map((x: unknown) => Number(x)),
        };
            setChartData(normalized)
            console.log(normalized)
        }
        catch (e) {
            console.log(e)
        }
        
        }
        fetchData();
        
    }
        , []);
    
    console.log("labels:", chartData?.labels?.length, chartData?.labels);
    console.log("totals:", chartData?.totals?.length, chartData?.totals);

    const totals = chartData?.totals ?? [];
    const colors = totals.map(v =>
    v >= 1000 ? "rgba(233, 11, 59, 0.6)" : "rgba(54, 235, 78, 0.6)"
  );
    const data = {
        labels: chartData?.labels  != undefined ? chartData.labels : [],
        datasets: [
            {
            label: chartData?.bucket != undefined ? IdToBucket[chartData.bucket] : "Unknown",
            data: (chartData?.totals ?? []),
            backgroundColor: colors,  
            borderColor: colors.map(c => c.replace("0.6", "1")),
            borderWidth: 1,
            
            },
        ],
    };
  
    return (
    <div className="chart-container">
            <h1>{chartData != undefined ? IdToBucket[chartData?.bucket] : "" } expenses of the month</h1>
        <Bar
            data={data}
                options={{
                    scales: {
                        y: {
                        type: "logarithmic",
                    },
                },
                plugins: {
                    title: {
                        display: false,
                        text: ""
                    },
                    legend: {
                        display: false
                    }
                }
            }
            }/>
    </div>
    )
}

export default ExpenseChart;