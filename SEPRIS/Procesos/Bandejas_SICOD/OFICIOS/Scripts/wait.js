var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_initializeRequest(InitializeRequest);
prm.add_endRequest(EndRequest);

function InitializeRequest(sender, args) {
    $get('Container').style.cursor = 'wait';
}

function EndRequest(sender, args) {
    $get('Container').style.cursor = 'auto';
}