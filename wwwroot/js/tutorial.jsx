var SalesApp = React.createClass({
    render: function() {
      return (
       <div className="App">
        <Chart chartData={this.state.chartData} />
        <BookSaleChart bookData={this.state.bookData} />
       </div>
      );
    }
  });
  ReactDOM.render(
    <SalesApp />,
    document.getElementById('content')
  );