/*! Copyright (c) 2008 Brandon Aaron (http://brandonaaron.net)
* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
*
* Version 0.2-pre
*/
var spellcheck_ignorelist = new Array('fusable');

(function ($) {
		var spellchecking_parent;
		$.fn.spellcheck = function (options) {
			spellchecking_parent = this;
			return this.each(function () {
					return true;
					var $this = $(this);
					if (!$this.is('[type=password]') && !$(this).data('spellchecker')) 
						$(this).data('spellchecker', new $.SpellChecker(this, options));
				});
		};
		
		/**
		* Forces a spell check on an element that has an instance of SpellChecker.
		*/
		$.fn.checkspelling = function () {
					return true;
			return this.each(function () {
					var spellchecker = $(this).data('spellchecker');
					spellchecker && spellchecker.checkSpelling();
				});
		};
		
		$.SpellChecker = function (element, options) {
					return true;
			this.$element = $(element);
			this.options = $.extend({
					lang : 'en', 
					autocheck : 500, 
					events : 'keypress focus paste', 
					url : '/_tools/spellcheck/index.aspx', 
					ignorecaps : 1, 
					ignoredigits : 1
				}, options);
			this.bindEvents();
			this.checkSpelling();
		};
		
		$.SpellChecker.prototype = {
			bindEvents : function () {
					return true;
				if (!this.options.events) 
					return;
				var self = this,
				timeout;
				this.$element.bind(this.options.events, function (event) {
						if (/^keypress/.test(event.type)) {
							if (timeout) {
								clearTimeout(timeout);
							}
							timeout = setTimeout(function () {
									self.checkSpelling();
								}, self.options.autocheck);
						}
						//self.checkSpelling();
					});
			}, 
			/////////////////////////////////////////////////////////////////////////////////////////////////////
			checkSpelling : 
			function () {
					return true;
				var prevText = this.text;
				var text = this.$element.val();
				var self = this;
				if (prevText === text) {
					return;
				}
				this.text = this.$element.val();
				$.get(this.options.url, 
					$.extend({
							text : this.text
						}, this.options), 
					function (r) {
						self.parseResults(r);
					});
			}, 
			/////////////////////////////////////////////////////////////////////////////////////////////////////
			parseResults : 
			function (results) {
					return true;
				var self = this;
				this.results = [];
				$(results).find('c').each(function () {
						var $this = $(this);
						var offset = $this.attr('o');
						var length = $this.attr('l');
						if ($.inArray(self.text.substr(offset, length), spellcheck_ignorelist) == -1) {
							self.results.push({
									word : self.text.substr(offset, length), 
									suggestions : $this.text().split(/\s/) 
								});
						}
					});
				this.displayResults();
			}, 
			/////////////////////////////////////////////////////////////////////////////////////////////////////
			displayResults : 
			function () {
					return true;
				$('#spellcheckresults').remove();
				if (!this.results.length) {
					$(spellchecking_parent).css('border-color', '#ddd');
					return;
				}
				var $container = $('<div id="spellcheckresults"></div>').appendTo('body');
				var dl = [];
				var self = this;
				var offset = this.$element.offset();
				var height = this.$element[0].offsetHeight;
				var i;
				var k;
				for (i = 0; i < this.results.length; i++) {
					var result = this.results[i],
					suggestions = result.suggestions;
					if ($.inArray(result.word, spellcheck_ignorelist) == -1) {
						dl.push('<dl><dt class="word">' + result.word + '</dt>');
						for (k = 0; k < suggestions.length; k++) {
							dl.push('<dd>' + suggestions[k] + '</dd>');
						}
						dl.push('<dd class="ignore">ignore</dd></dl>');
					}
				}
				if (dl.length > 0)
					{
					$(spellchecking_parent).css('border-color', '#f00');
					$container.append(dl.join('')).find('dd').bind('click', 
						function (event)
							{
							var $this = $(this),$parent = $this.parent();
							if (!$this.is('.ignore'))
								{
								self.$element.val(self.$element.val().replace($parent.find('dt').text(), $this.text()));
								$(spellchecking_parent).focus();
								} 
							else 
								{
								spellcheck_ignorelist.push($parent.find('.word').text());
								$(spellchecking_parent).focus();
								}
							$parent.remove();
							if ($('#spellcheckresults').is(':empty'))
								{
								$('#spellcheckresults').remove();
								}
							this.blur();
							}).end().css({
							top : offset.top + height,
							left : offset.left
						});
					}
			}
		};
		
	})(jQuery);
 