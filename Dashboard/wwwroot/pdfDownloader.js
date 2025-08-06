<script>
window.pdfDownloader = (filename, dotNetStreamReference) => {
        dotNetStreamReference.arrayBuffer().then(buffer => {
            const blob = new Blob([buffer], { type: "application/pdf" });
            const url = URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = filename ?? "download.pdf";
            a.click();
            a.remove();
            URL.revokeObjectURL(url);
        });
}
</script>