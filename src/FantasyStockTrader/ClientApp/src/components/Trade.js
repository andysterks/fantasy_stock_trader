import React, { useState } from "react";
import axios from "../common/AuthInterceptor";
import { useNavigate } from "react-router-dom";

function Trade() {
  const [searchQuery, setSearchQuery] = useState("");
  const [companies, setCompanies] = useState([]);

  const navigate = useNavigate();

  const search = (e) => {
    e.preventDefault();
    axios
      .get("api/company", { params: { query: searchQuery } })
      .then((response) => setCompanies(response.data));
  };

  const navigateToBuy = (symbol) => {
    navigate(`/trade/buy/${symbol}`);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-6">Trade</h1>
      <p className="text-lg mb-6">Search for symbol ⇒ Buy or sell</p>
      <div className="bg-white shadow-md rounded-lg p-6 mb-6">
        <h2 className="text-xl font-semibold mb-4">Search</h2>
        <form onSubmit={search} className="flex mb-4">
          <input
            type="text"
            placeholder="Enter company symbol"
            onChange={(e) => setSearchQuery(e.target.value)}
            value={searchQuery}
            className="flex-grow p-2 border border-gray-300 rounded-l-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            type="submit"
            className="bg-blue-500 text-white px-4 py-2 rounded-r-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            Search
          </button>
        </form>
        <div>
          <ul className="divide-y divide-gray-200">
            {companies.map((c) => (
              <li
                key={c.symbol}
                onClick={() => navigateToBuy(c.symbol)}
                className="py-3 cursor-pointer hover:bg-gray-100 transition duration-150 ease-in-out"
              >
                <span className="font-medium">{c.name}</span>{" "}
                <span className="text-gray-600">({c.symbol})</span>
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="bg-white shadow-md rounded-lg p-6">
        <h2 className="text-xl font-semibold mb-4">Available Cash</h2>
        <p className="text-lg">$10,000.00</p>
      </div>
    </div>
  );
}

export default Trade;
