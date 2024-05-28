import React, { useEffect, useState } from "react";
import axios from "../common/AuthInterceptor";

function Dashboard() {
  const [accountSummary, setAccountSummary] = useState([]);

  useEffect(() => {
    axios
      .get("api/accounts/summary")
      .then((response) => setAccountSummary(response.data));
  }, []);

  console.log(accountSummary);

  const formatCurrency = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  });

  return (
    <div>
      <div>Dashboard</div>
      <div>
        <h3>Account Value</h3>
        <p>{formatCurrency.format(accountSummary?.accountValue)}</p>
        <h3>Account Cost Basis</h3>
        <p>{formatCurrency.format(accountSummary?.accountCostBasis)}</p>
        <h3>Account Performance</h3>
        <p
          className={
            accountSummary?.accountPerformance > 0
              ? "text-success"
              : "text-danger"
          }
        >
          {accountSummary?.accountPerformance >= 0 ? "+" : "-"}
          {formatCurrency.format(Math.abs(accountSummary?.accountPerformance))}
        </p>
        <h3>Cash on Hand</h3>
        <p>{formatCurrency.format(accountSummary?.walletAmount)}</p>
        <h3>Holdings</h3>
        <ul>
          {accountSummary?.holdings?.map((h, i) => (
            <li key={i}>
              {h.symbol} - Shares: {h.sharesAmoount} - Value:{" "}
              {formatCurrency.format(h.value)} - Cost Basis:{" "}
              {formatCurrency.format(h.costBasis)} - Performance:{" "}
              <span
                className={h.performance > 0 ? "text-success" : "text-danger"}
              >
                {h.performance >= 0 ? "+" : "-"}{formatCurrency.format(Math.abs(h.performance))}
              </span>
            </li>
          ))}
        </ul>
        <h3></h3>
      </div>
    </div>
  );
}

export default Dashboard;
