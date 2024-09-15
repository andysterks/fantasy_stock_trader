import React, { useEffect, useState } from "react";
import axios from "../common/AuthInterceptor";

function Dashboard() {
  const [accountSummary, setAccountSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    fetchAccountSummary(currentPage);
  }, [currentPage]);

  const fetchAccountSummary = (page) => {
    setLoading(true);
    axios
      .get(`api/accounts/summary?page=${page}&pageSize=8`)
      .then((response) => {
        setAccountSummary(response.data);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching account summary:", error);
        setLoading(false);
      });
  };

  const formatCurrency = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  });

  const renderHoldingsRows = () => {
    if (accountSummary?.holdings.length === 0) { 
      return (
        <tr>
          <td colSpan="4" className="text-left px-4 py-2">No holdings found</td>
        </tr>
      );
    }

    return (
      accountSummary?.holdings?.map((h, i) => (
        <tr key={i} className={i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
          <td className="px-4 py-2 font-medium">{h.symbol}</td>
          <td className="px-4 py-2 text-right">{h.sharesAmount}</td>
          <td className="px-4 py-2 text-right">{formatCurrency.format(h.value)}</td>
          <td className={`px-4 py-2 text-right font-medium ${h.performance >= 0 ? 'text-green-600' : 'text-red-600'}`}>
            {h.performance >= 0 ? '+' : '-'}
            {formatCurrency.format(Math.abs(h.performance))}
          </td>
        </tr>
      ))
    );
  };

  const renderPagination = () => {
    if (!accountSummary || accountSummary.totalPages <= 1) return null;

    return (
      <div className="flex justify-center mt-4">
        <button
          onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
          disabled={currentPage === 1}
          className="px-4 py-2 mr-2 bg-blue-500 text-white rounded disabled:bg-gray-300"
        >
          Previous
        </button>
        <span className="px-4 py-2">
          Page {currentPage} of {accountSummary.totalPages}
        </span>
        <button
          onClick={() => setCurrentPage(prev => Math.min(prev + 1, accountSummary.totalPages))}
          disabled={currentPage === accountSummary.totalPages}
          className="px-4 py-2 ml-2 bg-blue-500 text-white rounded disabled:bg-gray-300"
        >
          Next
        </button>
      </div>
    );
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-spin rounded-full h-32 w-32 border-t-2 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8 text-center">Dashboard</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-semibold mb-4">Account Summary</h2>
          <div className="space-y-4">
            <div>
              <h3 className="text-lg font-medium text-gray-700">Account Value</h3>
              <p className="text-2xl font-bold text-blue-600">{formatCurrency.format(accountSummary?.accountValue)}</p>
            </div>
            <div>
              <h3 className="text-lg font-medium text-gray-700">Account Cost Basis</h3>
              <p className="text-2xl font-bold text-gray-800">{formatCurrency.format(accountSummary?.accountCostBasis)}</p>
            </div>
            <div>
              <h3 className="text-lg font-medium text-gray-700">Account Performance</h3>
              <p className={`text-2xl font-bold ${accountSummary?.accountPerformance >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {accountSummary?.accountPerformance >= 0 ? '+' : '-'}
                {formatCurrency.format(Math.abs(accountSummary?.accountPerformance))}
              </p>
            </div>
            <div>
              <h3 className="text-lg font-medium text-gray-700">Cash on Hand</h3>
              <p className="text-2xl font-bold text-green-600">{formatCurrency.format(accountSummary?.walletAmount)}</p>
            </div>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-semibold mb-4">Holdings</h2>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="bg-gray-100">
                  <th className="px-4 py-2 text-left">Symbol</th>
                  <th className="px-4 py-2 text-right">Shares</th>
                  <th className="px-4 py-2 text-right">Value</th>
                  <th className="px-4 py-2 text-right">Performance</th>
                </tr>
              </thead>
              <tbody>
                {renderHoldingsRows()}
              </tbody>
            </table>
          </div>
          {renderPagination()}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
