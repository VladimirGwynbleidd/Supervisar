
function validaLimite(obj, maxchar, label) {

    if (this.id) obj = this;

    var remaningChar = maxchar - obj.value.length;
    document.getElementById(label).innerHTML = remaningChar;

    if (remaningChar <= 0) {
        document.getElementById(label).innerHTML = 0;
        obj.value = obj.value.substring(maxchar, 0);
        return false;
    }
    else
    { return true; }


}