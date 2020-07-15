$(function() {
    var tagHelperVariants = $("a[data-tab$='-th']");
    tagHelperVariants.click(function() {
        instigator = this;
        tagHelperVariants.each(function() {
            var $tab = $(this);
            if ($tab.attr("aria-selected") !== "true" && this !== instigator) {
                this.click();
            }
        });
    });

    var htmlHelperVariants = $("a[data-tab$='-hh']");
    htmlHelperVariants.click(function() {
        instigator = this;
        htmlHelperVariants.each(function() {
            var $tab = $(this);
            if ($tab.attr("aria-selected") !== "true" && this !== instigator) {
                this.click();
            }
        });
    });

    var copyToClipboard = function(text) {
        // Create a textblock and assign the text and add to document
        var el = document.createElement('textarea');
        el.value = text;
        document.body.appendChild(el);
        el.style.display = "block";
    
        // select the entire textblock
        if (window.document.documentMode)
            el.setSelectionRange(0, el.value.length);
        else
            el.select();
        
        // copy to clipboard
        document.execCommand('copy');
        
        // clean up element
        document.body.removeChild(el);
    }

    $("code.hljs").each(function() {
        var $this = $(this);
        var language = /lang-(.+?)(\s|$)/.exec($this.attr("class"))[1].toUpperCase();
        if (language === 'CS') {
            language = "C#";
        }
        if (language === 'JS') {
            language = "JavaScript";
        }
        var $codeHeader = $(
            '<div class="code-header">'+
            '    <span class="language">'+ language +'</span>'+
            '    <button type="button" class="action" aria-label="Copy code">'+
            '		<span class="icon"><span class="glyphicon glyphicon-duplicate" role="presentation"></span></span>'+
            '		<span>Copy</span>'+
            '		<div class="successful-copy-alert is-transparent" aria-hidden="true">'+
            '			<span class="icon is-size-large">'+
            '				<span class="glyphicon glyphicon-ok" role="presentation"></span>'+
            '			</span>'+
            '		</div>'+
            '	</button>'+
            '</div>'
        );
        $this.closest("pre").before($codeHeader);
        $codeHeader.find("button").click(function() {
            copyToClipboard($this.closest("pre").text());
            var successAlert = $(this).find(".successful-copy-alert");
            successAlert.removeClass("is-transparent");
            setTimeout(function() {successAlert.addClass("is-transparent");}, 1000);
        });
    });
});