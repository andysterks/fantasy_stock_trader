# Fantasy Stock Trading App

<br>

_Software where you can trade stocks without having real money!_

<br>

Sign up for account and start trading [here](https://github.com/andysterks)

<br>

<image src="tbd.png">

## Local Installation Instructions

1. Install docker CLI
2. On command line run `docker pull postgres`
3. On command line run `docker build -t "fantasy_stock_trader" ./database`
4. On command line run `docker run --name fantasy_stock_trader -p 5432:5432 -d fantasy_stock_trader`

## Summary

Migrations command: dotnet ef migrations add MigrationNameHere
DB-Update Command: dotnet ef database update --connection 'connstring' --verbose

## Author

- **Andy Sterkowitz** - _Full-Stack Software Developer_ - [Website](https://andysterkowitz.com) | [LinkedIn](https://www.linkedin.com/in/andrewsterkowitz/)
