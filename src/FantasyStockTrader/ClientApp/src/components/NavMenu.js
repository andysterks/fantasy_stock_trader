import React, { useState } from 'react';
import { Link } from 'react-router-dom';

export function NavMenu() {
  const [isOpen, setIsOpen] = useState(false);

  const toggleNavbar = () => {
    setIsOpen(!isOpen);
  };

  return (
    <header className="bg-white shadow-md">
      <nav className="container mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center py-3">
          <Link to="/" className="text-xl font-bold text-gray-800">FantasyStockTrader</Link>
          <div className="sm:hidden">
            <button
              onClick={toggleNavbar}
              className="text-gray-500 hover:text-gray-600 focus:outline-none focus:text-gray-600"
              aria-label="Toggle menu"
            >
              <svg viewBox="0 0 24 24" className="h-6 w-6 fill-current">
                <path
                  fillRule="evenodd"
                  d="M4 5h16a1 1 0 0 1 0 2H4a1 1 0 1 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2z"
                />
              </svg>
            </button>
          </div>
          <div className={`sm:flex ${isOpen ? 'block' : 'hidden'}`}>
            <div className="px-2 pt-2 pb-4 sm:flex sm:p-0">
              <Link
                to="/dashboard"
                className={`block px-2 py-1 text-gray-700 font-semibold rounded hover:bg-gray-100 sm:mt-0 sm:ml-2 ${
                  window.location.pathname === '/dashboard' ? 'bg-gray-200' : ''
                }`}
              >
                Dashboard
              </Link>
              <Link
                to="/trade"
                className={`mt-1 block px-2 py-1 text-gray-700 font-semibold rounded hover:bg-gray-100 sm:mt-0 sm:ml-2 ${
                  window.location.pathname === '/trade' ? 'bg-gray-200' : ''
                }`}
              >
                Trade
              </Link>
            </div>
          </div>
        </div>
      </nav>
    </header>
  );
}
